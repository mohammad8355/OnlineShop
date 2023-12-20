using BusinessEntity.Models;
using BusinessLogicLayer.CategoryService;
using Microsoft.AspNetCore.Mvc;

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
            return View();
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


    }
}
