using BusinessLogicLayer.BrandService;
using DataAccessLayer.Models;
using Infrustructure.uploadfile;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    public class BrandController : Controller
    {
        private readonly BrandLogic brandLogic;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UploadFile FileManager;
        public BrandController(BrandLogic brandLogic, IWebHostEnvironment webHostEnvironment,UploadFile FileManager)
        {
            this.brandLogic = brandLogic;
            this.FileManager = FileManager;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var model = await brandLogic.BrandList();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddBrand()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBrand(BrandViewModel model)
        {
            ModelState.Remove("file");
            if (ModelState.IsValid)
            {
                var brand = new Brand()
                {
                    Name = model.Name,
                    rate = model.rate,
                };
                var result = await brandLogic.AddBrand(brand);
                if (result)
                {
                    if(model.file != null)
                    {
                        var destination = "Image/Brand/" + model.Name;
                        var root = webHostEnvironment.WebRootPath;
                        var completePath = Path.Combine(root, destination);
                        var uploadRes = await FileManager.Upload(model.Name, completePath, 0, null, model.file);
                        if ((bool)uploadRes.First())
                        {
                            brand.ImageName = model.Name + uploadRes.Last().ToString();
                            brandLogic.EditBrand(brand);
                            ViewBag.success = "برند مورد نطر با موفقیت افزوده شد";
                            return View();
                        }
                        ModelState.AddModelError("", "مشکل در افزودن فایل عکس");
                    }
                    else
                    {
                        ViewBag.success = "برند مورد نطر با موفقیت افزوده شد";
                        return View();
                    }
                }
            }
            return View(model);
        }
        public async Task<ActionResult> EditBrand(int Id)
        {
            var brand = await brandLogic.BrandDetail(Id);
            var model = new BrandViewModel()
            {
                Name = brand.Name,
                rate = brand.rate,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditBrand(BrandViewModel model)
        {
            ModelState.Remove("file");
            if (ModelState.IsValid)
            {
                var brand = await brandLogic.BrandDetail(model.Id);
                brand.Name = model.Name;
                brand.rate = model.rate;
                var result = brandLogic.EditBrand(brand);
                if (result)
                {
                    if(model.file != null)
                    {
                        var destination = "Image/Brand/" + model.Name;
                        var root = webHostEnvironment.WebRootPath;
                        var completePath = Path.Combine(root, destination);
                        var uploadRes = await FileManager.Upload(model.Name, completePath, 0, null, model.file);
                        if ((bool)uploadRes.First())
                        {
                            brand.ImageName = model.Name + uploadRes.Last().ToString();
                            brandLogic.EditBrand(brand);
                            return RedirectToAction("Index");
                        }
                        ModelState.AddModelError("", "مشکل در افزودن فایل عکس");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteBrand(int Id)
        {
            var result = await brandLogic.DeleteBrand(Id);
            return Json(result);
        }
    }
}
