
using BusinessLogicLayer.CategoryService;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.services;
using DataAccessLayer.Models;
using Microsoft.CodeAnalysis;
using PresentationLayer.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryLogic categoryLogic;

        public CategoryController(CategoryLogic categoryLogic)
        {
            this.categoryLogic = categoryLogic;

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
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    Name = model.Name,
                    Description = model.Description,
                    ParentId = model.ParentId,
                };
                var result = await categoryLogic.AddCategory(category);
                if (result)
                {
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
        public IActionResult EditCategory(AddEditCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    ParentId = model.ParentId,
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
            var Result = await categoryLogic.DeleteCategory(Id);
            return Json(new { result = Result });
        }
    }
}
