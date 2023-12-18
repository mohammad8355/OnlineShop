using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddCategory() 
        {
            return View();
        }

    }
}
