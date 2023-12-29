using BusinessEntity.Models;
using BusinessLogicLayer.CategoryService;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.services;
using BusinessEntity;
using Microsoft.CodeAnalysis;
using PresentationLayer.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    public class CategoryController : Controller
    {
        private readonly CategoryLogic categoryLogic;
        private readonly SubCategoryLogic subCategoryLogic;
        public CategoryController(CategoryLogic _categoryLogic, SubCategoryLogic subCategoryLogic)
        {
            categoryLogic = _categoryLogic;
            this.subCategoryLogic = subCategoryLogic;

        }
        public IActionResult Index()
        {
            var model = categoryLogic.CategoryList();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category model)
        {
            if (await categoryLogic.AddCategory(model))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "عملیات افزودن با شکست مواجه شد");
                return View(model);
            }

        }
        [HttpGet]
        public async Task<IActionResult> EditCategory(int Id)
        {
            var model = categoryLogic.CategoryDetail(Id);
            return View("AddCategory", model);
        }
        [HttpPost]
        public async Task<IActionResult> EditCategory(Category model)
        {
            var resualt = categoryLogic.UpdateCategory(model);
            if (resualt.Result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "ویرایش دسته بندی با خطا مواجه شد");
                return View("AddCategory", model);
            }

        }
        [HttpGet("Category/DeleteCategory/Id")]
        public async Task<JsonResult> DeleteCategory(string Id)
        {
            int cId = int.Parse(Id);
            var category = categoryLogic.CategoryDetail(cId);
            var resualt = await categoryLogic.DeleteCategory(cId);
            return Json(new { name = category.Name, Resualt = resualt });
        }
        [HttpGet]
        public IActionResult AddSubCategory()
        {
            var model = new AddEditSubCategoryViewModel();
            model.categories = categoryLogic.CategoryList();
            ViewBag.categories = new SelectList(model.categories, "Id", "Name");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddSubCategory(AddEditSubCategoryViewModel model)
        {
            var category = categoryLogic.CategoryDetail(model.subCategory.category_Id);
            model.subCategory.Parent = category.IdentifierName;
            model.subCategory.category = category;
            if (await subCategoryLogic.AddSubCategory(model.subCategory))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "عملیات افزودن با شکست مواجه شد");
                return View(model);
            }

        }
        [HttpGet]
        public async Task<IActionResult> EditSubCategory(int Id)
        {
            var model = new AddEditSubCategoryViewModel();
            model.subCategory = subCategoryLogic.SubCategoryDetail(Id);
            model.categories = categoryLogic.CategoryList();
            ViewBag.categories = new SelectList(model.categories, "Id", "Name", model.subCategory.category_Id);
            return View("AddSubCategory", model);
        }
        [HttpPost]
        public async Task<IActionResult> EditSubCategory(AddEditSubCategoryViewModel model)
        {
            var category = categoryLogic.CategoryDetail(model.subCategory.category_Id);
            model.subCategory.Parent = category.IdentifierName;
            model.subCategory.category = category;
            var resualt = subCategoryLogic.UpdateSubCategory(model.subCategory);
            if (resualt.Result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "ویرایش دسته بندی با خطا مواجه شد");
                return View("AddSubCategory", model);
            }

        }
        public async Task<IActionResult> SubCategoryList()
        {
            var model = subCategoryLogic.SubCategoryList();
            return View(model);
        }
        [HttpGet("Category/DeleteSubCategory/Id")]
        public async Task<JsonResult> DeleteSubCategory(string Id)
        {
            int cId = int.Parse(Id);
            var subcategory = subCategoryLogic.SubCategoryDetail(cId);
            var resualt = await subCategoryLogic.DeleteSubCategory(cId);
            return Json(new { name = subcategory.Name, Resualt = resualt });
        }
        public async Task<IActionResult> CategoryChart()
        {
            return View();
        }
        [HttpGet("Category/chart")]
        public JsonResult chart()
        {
            var model = categoryLogic.CategoryList();
            return Json(model);
        }


    }
}
