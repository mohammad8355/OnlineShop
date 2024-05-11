using BusinessLogicLayer.OrderDetailsService;
using BusinessLogicLayer.OrderService;
using BusinessLogicLayer.ProductService;
using DataAccessLayer.Models;
using Infrustructure.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PresentationLayer.Models.ViewModels;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using Utility.ProductCodeGenerator;
using ZarinPal.Class;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class ShopController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ProductLogic productLogic;
        private readonly ProductCodeGenerator _codeGenerator;
        private readonly OrderLogic orderLogic;
        private readonly OrderDetailsLogic orderDetailsLogic;
        private readonly IPayment _payment;
        public ShopController(ProductCodeGenerator codeGenerator,UserManager<ApplicationUser> _userManager, ProductLogic productLogic, OrderLogic orderLogic, OrderDetailsLogic orderDetailsLogic, ZarinPalPay payment)
        {
            this._userManager = _userManager;
            this.productLogic = productLogic;
            this.orderLogic = orderLogic;
            this.orderDetailsLogic = orderDetailsLogic;
            _codeGenerator = codeGenerator;
            _payment = payment;
        }
        public async Task<IActionResult> Index(decimal  discount = 0)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var Order = orderLogic.OrderList().Where(o => o.User_Id == currentUser.Id && o.IsFinally == false).FirstOrDefault();
            if (Order != null)
            {
                var model = new BasketShopViewModel()
                {
                    Order = Order,
                };
                return View(model);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddToCart(int product_Id, int count)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var product = await productLogic.ProductDetail(product_Id);
            var discount = (decimal)product.Discount / 100;
            var price = product.Price - (discount * product.Price);
            var totalPrice = count * price;
            var Order = orderLogic.OrderList().Where(o => o.User_Id == currentUser.Id && o.IsFinally == false).FirstOrDefault();
            if (Order != null)
            {
                var orderDetail = Order.orderDetails.Where(o => o.order_Id == Order.Id && o.Product_Id == product_Id).FirstOrDefault();
                if (orderDetail != null)
                {
                    orderDetail.count += count;
                    orderDetail.TotalPrice += totalPrice;
                }
                else
                {
                    var neworderDetail = new OrderDetails()
                    {
                        Product_Id = product_Id,
                        count = count,
                        TotalPrice = totalPrice,
                    };
                    Order.orderDetails.Add(neworderDetail);
                }
                Order.TotalPrice += totalPrice;
                Order.TotalCount += count;
                var result = orderLogic.EditOrder(Order);
                if (result)
                {
                    product.QuantityInStock -= count;
                    if (await productLogic.UpdateProduct(product))
                    {
                        return Json(new { message = "به سبد خرید اضافه شد" ,type = "success"});
                    }
                }
                return Json(new { message = "خطا" ,type = "error"});
            }
            else
            {
                var newOrder = new Order()
                {
                    User_Id = currentUser.Id,
                    TotalPrice = totalPrice,
                    TotalCount = count,
                    DateCreate = DateTime.Now,
                };
                var result = await orderLogic.CreateOrder(newOrder);
                if (result)
                {
                    var orderDetail = new OrderDetails()
                    {
                        order_Id = newOrder.Id,
                        TotalPrice = totalPrice,
                        count = count,
                        Product_Id = product_Id,
                    };
                    var resultOrderDetail = orderDetailsLogic.EditOrderDetails(orderDetail);
                    if (resultOrderDetail)
                    {
                        product.QuantityInStock -= count;
                        if (await productLogic.UpdateProduct(product))
                        {
                            return Json(new { message = "به سبد خرید اضافه شد" , type = "success" });
                        }
                    }
                }
                return Json(new { message = "خطا" , type = "error" });
            }


        }
        [HttpGet]
        public async Task<JsonResult> DeleteOrderDetail(int Id, int orderId)
        {
            var order = orderLogic.OrderDetail(orderId);
            if (order.orderDetails.Count() > 1)
            {
                var orderDetail = order.orderDetails.Where(o => o.Id == Id).FirstOrDefault();
                if (orderDetail != null)
                {
                    var product = await productLogic.ProductDetail(orderDetail.Product_Id);
                    var result = await orderDetailsLogic.DeleteOrderDetails(Id);
                    if (result)
                    {
                        product.QuantityInStock += orderDetail.count;
                        order.TotalCount -= orderDetail.count;
                        order.TotalPrice -= orderDetail.TotalPrice;
                        if (await productLogic.UpdateProduct(product))
                        {
                            return Json(new { message = "سفارش با موفقیت حذف شد", header = "موفقیت", type = "success" });
                        }
                    }
                }
                return Json(new { message = "خطا ", header = "هشدار خطا", type = "warning" });
            }
            else
            {
                var orderDetail = order.orderDetails.Where(o => o.Id == Id).FirstOrDefault();
                var product = await productLogic.ProductDetail(orderDetail.Product_Id);
                if (await orderDetailsLogic.DeleteOrderDetails(Id))
                {
                    if (await orderLogic.DeleteOrder(orderId))
                    {
                        product.QuantityInStock += orderDetail.count;
                        if (await productLogic.UpdateProduct(product))
                        {
                            return Json(new { message = "سفارش با موفقیت حذف شد", header = "موفقیت", type = "success" });
                        }
                    }
                }
                return Json(new { message = "خطا ", header = "هشدار خطا", type = "warning" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> AddCountOrderDetails(int Id)
        {
            var orderDetail = orderDetailsLogic.OrderDetailsDetail(Id);
            if (orderDetail != null)
            {
                var order = orderLogic.OrderDetail(orderDetail.order_Id);
                var product = orderDetail.Product;
                if (orderDetail.count < orderDetail.Product.QuantityInStock)
                {
                    var discount = (decimal)orderDetail.Product.Discount / 100;
                    var price = orderDetail.Product.Price - (orderDetail.Product.Price * discount);
                    orderDetail.count += 1;
                    orderDetail.TotalPrice += price;
                    order.TotalCount += 1;
                    order.TotalPrice += price;
                    product.QuantityInStock -= 1;
                    var result = orderDetailsLogic.EditOrderDetails(orderDetail);
                    result = orderLogic.EditOrder(order);
                    result = await productLogic.UpdateProduct(product);
                    return Json(new { res = result, price = price });
                }
            }
            return Json(new { res = false });

        }
        [HttpGet]
        public async Task<IActionResult> MinusCountOrderDetails(int Id)
        {
            var orderDetail = orderDetailsLogic.OrderDetailsDetail(Id);
            if (orderDetail != null)
            {
                var order = orderLogic.OrderDetail(orderDetail.order_Id);
                var product = orderDetail.Product;
                if (orderDetail.count > 1)
                {
                    var discount = (decimal)orderDetail.Product.Discount / 100;
                    var price = orderDetail.Product.Price - (orderDetail.Product.Price * discount);
                    orderDetail.count -= 1;
                    orderDetail.TotalPrice -= price;
                    order.TotalCount -= 1;
                    order.TotalPrice -= price;
                    product.QuantityInStock += 1;
                    var result = orderDetailsLogic.EditOrderDetails(orderDetail);
                    result = orderLogic.EditOrder(order);
                    result = await productLogic.UpdateProduct(product);
                    return Json(new { res = result, price = price });
                }
            }
            return Json(new { res = false });

        }
        [HttpGet]
        public async Task<IActionResult> OrderFinally()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var order = orderLogic.FindOpenOrderByUser(user.Id);
            var model = new OrderFinallyViewModel()
            {
                User = user,
                Order = order,
            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Pay(int order_Id)
        {
            var order = orderLogic.OrderDetail(order_Id);
            var description = $"خرید محصول توسط کاربر{order.User.FullName}";
            var callbackurl = "https://localhost:44337/Shop/Verification/" + order_Id;
            var paymentResult = await _payment.Pay("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", callbackurl, (int)order.TotalPrice, description,"sampleEmial@gmail.com","091234568");
            return Json(new { url = $"https://sandbox.zarinpal.com/pg/StartPay/{paymentResult.Authority}"});
        }
        public async Task<IActionResult> Verification(string authority, string status)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var order = orderLogic.FindOpenOrderByUser(user.Id);
            var verificationResult = await _payment.Verification((int)order.TotalPrice, authority, "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx");
            var model = new VerificationPaymentViewModel()
            {
                User = order.User,
                order = order,
                TrackingCode = verificationResult.RefId.ToString(),
                Status = verificationResult.Status.ToString(),
            };
            if (status.Equals("OK"))
            {
                order.IsFinally = true;
                var FactorNumber = _codeGenerator.CodeGenerator("Fact",false,16,false,16,true);
                order.FactorNumber = FactorNumber;
                order.TrackingCode = model.TrackingCode;
                order.PayDate = DateTime.Now;
                orderLogic.EditOrder(order);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult OrderDetail(int Id)
        {
            var order = orderLogic.OrderDetail(Id);
            var Products = order.orderDetails.Select(od => new { od.Product.Name, od.Product.ProductCode }).ToList();
            return Json(new { products = Products.ToArray(), price = order.TotalPrice, FactorNumber = order.FactorNumber, TrackingCode = order.TrackingCode, date = order.PayDate, customerName = order.User.UserName });
        }
    }
}
