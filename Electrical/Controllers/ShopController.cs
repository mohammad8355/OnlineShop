using BusinessLogicLayer.OrderDetailsService;
using BusinessLogicLayer.OrderService;
using BusinessLogicLayer.ProductService;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PresentationLayer.Models.ViewModels;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class ShopController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ProductLogic productLogic;
        private readonly OrderLogic orderLogic;
        private readonly OrderDetailsLogic orderDetailsLogic;
        public ShopController(UserManager<ApplicationUser> _userManager, ProductLogic productLogic, OrderLogic orderLogic, OrderDetailsLogic orderDetailsLogic)
        {
            this._userManager = _userManager;
            this.productLogic = productLogic;
            this.orderLogic = orderLogic;
            this.orderDetailsLogic = orderDetailsLogic;
        }
        public async Task<IActionResult> Index()
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
            var totalPrice = count * product.Price;
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
                    if(await productLogic.UpdateProduct(product))
                    {
                        return Json(new { message = "به سبد خرید اضافه شد" });
                    }
                }
                return Json(new { message = "خطا" });
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
                            return Json(new { message = "به سبد خرید اضافه شد" });
                        }
                    }
                }
                return Json(new { message = "خطا" });
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
                        if(await productLogic.UpdateProduct(product))
                        {
                            return Json(new { message = "سفارش با موفقیت حذف شد", header = "موفقیت", type = "success" });
                        }
                    }
                }
                return Json(new { message = "خطا ", header = "هشدار خطا" , type = "warning" });
            }
            else
            {
                if (await orderDetailsLogic.DeleteOrderDetails(Id))
                {
                    var orderDetail = order.orderDetails.Where(o => o.Id == Id).FirstOrDefault();
                    var product = await productLogic.ProductDetail(orderDetail.Product_Id);
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
        public IActionResult AddCountOrderDetails(int Id)
        {
            var orderDetail = orderDetailsLogic.OrderDetailsDetail(Id);
            if(orderDetail != null)
            {
                if(orderDetail.count < orderDetail.Product.QuantityInStock)
                {
                    var price = orderDetail.Product.Price;
                    orderDetail.count += 1;
                    orderDetail.TotalPrice += price;
                    orderDetail.order.TotalCount += 1;
                    orderDetail.order.TotalPrice += price;
                    orderDetail.Product.QuantityInStock -= 1;   
                    var result = orderDetailsLogic.EditOrderDetails(orderDetail);
                    return Json(new { res = result, price = price });
                }
            }
            return Json(new { res = false });
                
        }
        [HttpGet]
        public IActionResult MinusCountOrderDetails(int Id)
        {
            var orderDetail = orderDetailsLogic.OrderDetailsDetail(Id);
            if (orderDetail != null)
            {
                if(orderDetail.count > 0)
                {
                    var price = orderDetail.Product.Price;
                    orderDetail.count -= 1;
                    orderDetail.TotalPrice -= price;
                    orderDetail.order.TotalCount -= 1;
                    orderDetail.order.TotalPrice -= price;
                    orderDetail.Product.QuantityInStock += 1;
                    var result = orderDetailsLogic.EditOrderDetails(orderDetail);
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
        [HttpPost]
        public async Task<IActionResult> OrderFinally(OrderFinallyViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var order = orderLogic.FindOpenOrderByUser(user.Id);
            return View(model);
        }
    }
}
