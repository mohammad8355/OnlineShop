
using BusinessLogicLayer.CategoryService;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.services;
using DataAccessLayer.Models;
using Microsoft.CodeAnalysis;
using PresentationLayer.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Infrustructure.uploadfile;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryLogic categoryLogic;
        private readonly UploadFile _fileManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(CategoryLogic categoryLogic,UploadFile fileManager,IWebHostEnvironment webHostEnvironment)
        {
            this.categoryLogic = categoryLogic;
            _fileManager = fileManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var model = categoryLogic.CategoryList().ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(AddEditCategoryViewModel model)
        {
            var webrootpath = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
               
                var category = new Category()
                {
                    Name = model.Name,
                    Description = model.Description,
                    ParentId = model.ParentId,
                };
                var result = await categoryLogic.AddCategory(category);
                if (result != 0)
                {
                    if (model.Cover != null)
                    {
                        var limitsize = 0;
                        var destination = Path.Combine(webrootpath, "Image","Category");
                        var coverName = result;
                         var uploadResult =  await _fileManager.Upload(result.ToString(),destination, limitsize, null, model.Cover);
                         if (uploadResult.result)
                         {
                             var res = await categoryLogic.UpdateCoverCategory(result, coverName + uploadResult.message.ToString());
                             if (!res) return View(model);
                         }
                    }
                    ViewBag.success = "با موفقیت انجام شد";
                    return View();
                }
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult EditCategory(int Id)
        {
            
            var cateogry = categoryLogic.CategoryDetail(Id);
            var model = new AddEditCategoryViewModel()
            {
                Id = cateogry.Id,
                Name = cateogry.Name,
                Description = cateogry.Description,
                ParentId = cateogry.ParentId,
            };
            return View("AddCategory",model);
        }
        [HttpPost]
        public async Task<IActionResult> EditCategory(AddEditCategoryViewModel model)
        {
            var webrootpath = _webHostEnvironment.WebRootPath;

            if (ModelState.IsValid)
            {
                var cover = "";
                if (model.Cover != null)
                {
                    var limitsize = 0;
                    var destination = Path.Combine(webrootpath, "Image","Category");
                    var coverName = model.Id;
                    var uploadResult =  await _fileManager.Upload(model.Id.ToString(),destination, limitsize, null, model.Cover);
                    if (uploadResult.result)
                    {
                        cover = coverName + uploadResult.message.ToString(); 
                    }
                }
                var category = new Category()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    ParentId = model.ParentId,
                    Cover = cover
                };
                var result = categoryLogic.UpdateCategory(category);
                if (result)
                {
                   return RedirectToAction(nameof(Index));
                }
                return View("AddCategory",model);
            }
            return View("AddCategory",model);
        }
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var webrootpath = _webHostEnvironment.WebRootPath;
            var category = categoryLogic.CategoryDetail(Id);
            var destination = "Image\\Category\\" + category.Name + "_" + category.Id;
            var completeDestination = Path.Combine(webrootpath, destination);
            await _fileManager.DeleteFile(completeDestination);
            var dirPath = "Image\\Category\\" + category.Name;
            _fileManager.DeleteDire(Path.Combine(webrootpath, dirPath));
            var Result = await categoryLogic.DeleteCategory(Id);
            return Json(new { result = Result });
        }
    }
}
