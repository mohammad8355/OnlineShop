
using BusinessLogicLayer.CategoryService;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.services;
using DataAccessLayer.Models;
using Microsoft.CodeAnalysis;
using PresentationLayer.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    public class CategoryController : Controller
    {
        private readonly CategoryLogic categoryLogic;

        public CategoryController(CategoryLogic categoryLogic)
        {
            this.categoryLogic = categoryLogic;

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
        #region old code
        //public IActionResult AddHeadCategory()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> AddHeadCategory(AddEditHeadCategoryViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var HeadCategoryModel = new HeadCategory()
        //        {
        //            Name = model.Name,
        //            Description = model.Description,
        //            IdentifierName = model.IdentifierName,
        //        };

        //        if (await headCategoryLogic.AddHeadCategory(HeadCategoryModel))
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "عملیات افزودن با شکست مواجه شد");
        //            return View(model);
        //        }

        //    }
        //    return View(model);

        //}
        //[HttpGet]
        //public IActionResult EditHeadCategory(int Id)
        //{
        //    var HeadCategoryModel = headCategoryLogic.HeadCategoryDetail(Id);
        //    var model = new AddEditHeadCategoryViewModel()
        //    {
        //        Name = HeadCategoryModel.Name,
        //        Description = HeadCategoryModel.Description,
        //        IdentifierName = HeadCategoryModel.IdentifierName,
        //    };
        //    return View("AddHeadCategory", model);
        //}
        //[HttpPost]
        //public IActionResult EditHeadCategory(AddEditHeadCategoryViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var HeadCategoryModel = headCategoryLogic.HeadCategoryDetail(model.Id);
        //        HeadCategoryModel.Name = model.Name;
        //        HeadCategoryModel.IdentifierName = model.IdentifierName;
        //        HeadCategoryModel.Description = model.Description;
        //        foreach(var cat in HeadCategoryModel.Categories)
        //        {
        //            cat.Parent = model.IdentifierName;
        //        }
        //        var resualt = headCategoryLogic.UpdateHeadCategory(HeadCategoryModel);
        //        if (resualt)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "ویرایش دسته بندی با خطا مواجه شد");
        //            return View("AddHeadCategory", model);
        //        }
        //    }
        //    return View("AddHeadCategory", model);

        //}
        //[HttpGet("Category/DeleteHeadCategory/Id")]
        //public async Task<JsonResult> DeleteHeadCategory(string Id)
        //{
        //    int cId = int.Parse(Id);
        //    var category = headCategoryLogic.HeadCategoryDetail(cId);
        //    var resualt = await headCategoryLogic.DeleteHeadCategory(cId);
        //    return Json(new { name = category.Name, Resualt = resualt });
        //}
        //[HttpGet]
        //public IActionResult AddCategory()
        //{
        //    var headcategories = headCategoryLogic.HeadCategoryList();
        //    ViewBag.categories = new SelectList(headcategories, "Id", "Name");
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> AddCategory(AddEditCategoryViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var headCategory = headCategoryLogic.HeadCategoryDetail(model.headCategory_Id);
        //        var CategoryModel = new Category()
        //        {
        //            Name = model.Name,
        //            Description = model.Description,
        //            IdentifierName = model.IdentifierName,
        //            headCategory_Id = model.headCategory_Id,
        //            Parent = headCategory.IdentifierName,
        //        };
        //        if (await categoryLogic.AddCategory(CategoryModel))
        //        {
        //            ViewBag.success = "عملیات افزودن با موقثیت انحام شد !";
        //            return View();
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "عملیات افزودن با شکست مواجه شد");
        //            return View(model);
        //        }
        //    }
        //    return View(model);


        //}
        //[HttpGet]
        //public IActionResult EditCategory(int Id)
        //{
        //    var CategoryModel = categoryLogic.CategoryDetail(Id);
        //    var Headcategories = headCategoryLogic.HeadCategoryList();
        //    ViewBag.categories = new SelectList(Headcategories, "Id", "Name", CategoryModel.headCategory_Id);
        //    var model = new AddEditCategoryViewModel()
        //    {
        //        Name = CategoryModel.Name,
        //        IdentifierName = CategoryModel.IdentifierName,
        //        Description = CategoryModel.Description,
        //        headCategory_Id = CategoryModel.headCategory_Id,
        //    };
        //    return View("AddCategory", model);
        //}
        //[HttpPost]
        //public IActionResult EditCategory(AddEditCategoryViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var CategoryModel = categoryLogic.CategoryDetail(model.Id);
        //        CategoryModel.Name = model.Name;
        //        CategoryModel.Description = model.Description;
        //        CategoryModel.IdentifierName = model.IdentifierName;
        //        foreach(var sub in CategoryModel.SubCategories)
        //        {
        //            sub.Parent = model.IdentifierName;
        //        }
        //        CategoryModel.Parent = CategoryModel.headCategory.IdentifierName;
        //        CategoryModel.headCategory_Id = model.headCategory_Id;
        //        var resualt = categoryLogic.UpdateCategory(CategoryModel);
        //        if (resualt)
        //        {
        //            return RedirectToAction("CategoryList");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "ویرایش دسته بندی با خطا مواجه شد");
        //            return View("AddCategory", model);
        //        }
        //    }
        //    return View("AddCategory", model);

        //}
        //[HttpGet("Category/DeleteCategory/Id")]
        //public async Task<JsonResult> DeleteCategory(string Id)
        //{
        //    int cId = int.Parse(Id);
        //    var category = categoryLogic.CategoryDetail(cId);
        //    var resualt = await categoryLogic.DeleteCategory(cId);
        //    return Json(new { name = category.Name, Resualt = resualt });
        //}
        //public IActionResult CategoryList()
        //{
        //    var model = categoryLogic.CategoryList();
        //    return View(model);
        //}
        //[HttpGet("Category/DeleteSubCategory/Id")]
        //public async Task<JsonResult> DeleteSubCategory(string Id)
        //{
        //    int cId = int.Parse(Id);
        //    var subcategory = subCategoryLogic.SubCategoryDetail(cId);
        //    var resualt = await subCategoryLogic.DeleteSubCategory(cId);
        //    return Json(new { name = subcategory.Name, Resualt = resualt });
        //}
        //public IActionResult CategoryChart()
        //{
        //    return View();
        //}
        //[HttpGet("Category/chart")]
        //public JsonResult chart()
        //{
        //    var model = headCategoryLogic.HeadCategoryList();
        //    return Json(model);
        //}
        //[HttpGet]
        //public IActionResult AddSubCategory()
        //{
        //    var headcategories = headCategoryLogic.HeadCategoryList();
        //    ViewBag.categories = new SelectList(headcategories, "Id", "Name");
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> AddSubCategory(AddEditSubCategoryViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var category = categoryLogic.CategoryDetail(model.category_Id);
        //        var SubCategoryModel = new Category()
        //        {
        //            Name = model.Name,
        //            Description = model.Description,
        //            IdentifierName = model.IdentifierName,
        //            Parent = category.IdentifierName,
        //            category_Id = model.category_Id,
        //        };
        //        if (await subCategoryLogic.AddSubCategory(SubCategoryModel))
        //        {
        //            ViewBag.success = "عملیات افزودن با موقثیت انحام شد !";
        //            return View(model);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "عملیات افزودن زیر دسته بندی با شکست مواجه شد");
        //            return View(model);
        //        }
        //    }
        //    return View(model);
        //}
        //public IActionResult EditSubCategory(int Id)
        //{
        //    var subCategoryModel = subCategoryLogic.SubCategoryDetail(Id);
        //    var headcategories = headCategoryLogic.HeadCategoryList();
        //    ViewBag.categories = new SelectList(headcategories, "Id", "Name", subCategoryModel.category.headCategory_Id);
        //    var model = new AddEditSubCategoryViewModel()
        //    {
        //        Name = subCategoryModel.Name,
        //        Description = subCategoryModel.Description,
        //        IdentifierName = subCategoryModel.IdentifierName,
        //        category_Id = subCategoryModel.category_Id,
        //        Id = subCategoryModel.Id,
        //    };
        //    return View("AddSubCategory", model);
        //}
        //[HttpPost]
        //public async Task<IActionResult> EditSubCategory(AddEditSubCategoryViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var SubCategoryModel = subCategoryLogic.SubCategoryDetail(model.Id);
        //        SubCategoryModel.Name = model.Name;
        //        SubCategoryModel.Description = model.Description;
        //        SubCategoryModel.IdentifierName = model.IdentifierName;
        //        SubCategoryModel.Parent = SubCategoryModel.category.IdentifierName;
        //        SubCategoryModel.category_Id = model.category_Id;

        //        if (await subCategoryLogic.AttachSubCategory(SubCategoryModel))
        //        {

        //            return RedirectToAction("SubCategoryList");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "عملیات ویرایش زیر دسته بندی با شکست مواجه شد");
        //            return View("AddSubCategory", model);
        //        }
        //    }
        //    return View("AddSubCategory", model);
        //}
        //public IActionResult SubCategoryList()
        //{
        //    var model = subCategoryLogic.SubCategoryList();
        //    return View(model);
        //}
        //[HttpGet("Category/SubCategoryDropDown/Id")]
        //public JsonResult SubCategoryDropDown(int headCategory_Id)
        //{
        //    #region make list of categories and subcategories for create select list
        //    var categories = categoryLogic.CategoryList().Where(s => s.headCategory_Id == headCategory_Id).ToList();
        //    #endregion
        //    return Json(new SelectList(categories, "Id", "Name"));
        //}
        #endregion
    }
}
