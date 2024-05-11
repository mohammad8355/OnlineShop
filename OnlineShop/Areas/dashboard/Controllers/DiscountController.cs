using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.DiscountService;
using PresentationLayer.Models.ViewModels;
using BusinessLogicLayer.DiscountToProductService;
using BusinessLogicLayer.ProductService;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin")]
    public class DiscountController : Controller
    {
        private readonly DiscountLogic discountLogic;
        private readonly DiscountToProductLogic discountToProductLogic;
        private readonly ProductLogic productLogic;
        public DiscountController(ProductLogic productLogic, DiscountToProductLogic discountToProductLogic, DiscountLogic discountLogic)
        {
            this.discountLogic = discountLogic;
            this.discountToProductLogic = discountToProductLogic;
            this.productLogic = productLogic;
        }
        public async Task<IActionResult> Index()
        {
            var model = await discountLogic.DiscountList();
            return View(model.ToList());
        }
        [HttpGet]
        public IActionResult AddDiscount()
        {
            var products = productLogic.ProductList();
            var selectlist = new SelectList(products, "Id", "Name");
            var model = new AddEditDiscountViewModel()
            {
                discountToProducts = products.Select(p => p.Id).ToList(),
                selectLists = selectlist,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddDiscount(AddEditDiscountViewModel model)
        {
            model.CreateDate = DateTime.Now;
            ModelState.Remove("DiscountCode");
            ModelState.Remove("selectLists");
            ModelState.Remove("discountToProducts");
            if (ModelState.IsValid)
            {
                var discountModel = new Discount()
                {
                    Name = model.Name,
                    Value = model.Value,
                    DateBase = model.DateBase,
                    CreateDate = model.CreateDate,
                    DiscountCode = model.DiscountCode,
                    Duration = model.Duration,
                    Active = model.Active,
                };
                var result = await discountLogic.AddDiscount(discountModel);
                var discountId = discountLogic.DiscountList().Result.Where(d => d.Name == model.Name && d.DiscountCode == model.DiscountCode).Select(d => d.Id).FirstOrDefault();
                if (result)
                {
                    if (model.discountToProducts != null)
                        foreach (var product in model.discountToProducts)
                    {
                        var discountToProduct = new DiscountToProduct()
                        {
                            Discount_Id = discountId,
                            Product_Id = product,
                        };
                       await discountToProductLogic.AddDiscountToProduct(discountToProduct);
                    }
                    ViewBag.success = "تخفیف افزوده شد!";
                    return RedirectToAction("Index");
                }
                return View(model);

            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditDiscount(int Id)
        {
            var discount = await discountLogic.DiscountDetails(Id);
            var products = productLogic.ProductList();
            var selectlist = new SelectList(products, "Id", "Name");
            var model = new AddEditDiscountViewModel()
            {
                Name = discount.Name,
                CreateDate = discount.CreateDate,
                DateBase = discount.DateBase,
                Value = discount.Value,
                Active = discount.Active,
                DiscountCode = discount.DiscountCode,
                Duration = discount.Duration,
                discountToProducts = discount.discountToProducts.Select(dp => dp.Product_Id).ToList(),
                selectLists = selectlist,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditDiscount(AddEditDiscountViewModel model)
        {
            ModelState.Remove("DiscountCode");
            ModelState.Remove("selectLists");
            ModelState.Remove("discountToProducts");
            if (ModelState.IsValid)
            {
                var discount = await discountLogic.DiscountDetails(model.Id);
                discount.Name = model.Name;
                discount.CreateDate = model.CreateDate;
                discount.DateBase = model.DateBase;
                discount.Active = model.Active;
                discount.DiscountCode = model.DiscountCode;
                discount.Duration = model.Duration;
                discount.Value = model.Value;
                var result = await discountLogic.EditDiscount(discount);
                if (result)
                {
                    foreach(var dis in discountToProductLogic.DiscountToProductList())
                    {
                       await discountToProductLogic.DeleteDiscountToProduct(model.Id, dis.Product_Id);
                    };
                    if(model.discountToProducts != null)
                    foreach (var product in model.discountToProducts)
                    {
                        var discountToProduct = new DiscountToProduct()
                        {
                            Discount_Id = model.Id,
                            Product_Id = product,
                        };
                        await discountToProductLogic.AddDiscountToProduct(discountToProduct);
                    }
                   return RedirectToAction("Index");
                }
                ModelState.AddModelError("","خطا در ویرایش تخفیف");
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteDiscount(int Id)
        {
            var discount = await discountLogic.DiscountDetails(Id);
            var result = await discountLogic.DeleteDiscount(Id);
            return Json(new { name = discount.Name, result = result });
        }
    }
}
