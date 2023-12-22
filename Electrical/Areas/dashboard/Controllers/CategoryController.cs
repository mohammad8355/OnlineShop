using BusinessEntity.Models;
using BusinessLogicLayer.CategoryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    public class CategoryController : Controller
    {
        private readonly CategoryLogic categoryLogic;
        public CategoryController(CategoryLogic _categoryLogic)
        {
            categoryLogic = _categoryLogic;
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
            await categoryLogic.AddCategory(model);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditCategory(int Id)
        {
            var model = categoryLogic.CategoryDetail(Id);
            return View("AddCategory",model);
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
                return View("AddCategory",model);
            }

        }
        [HttpGet("Category/DeleteCategory/Id")]
        public async Task<JsonResult> DeleteCategory(string Id)
        {
            int cId = int.Parse(Id);
            var category = categoryLogic.CategoryDetail(cId);
           var resualt = await categoryLogic.DeleteCategory(cId);
            return Json(new { name = category.Name , Resualt = resualt});
        }



    }
}
