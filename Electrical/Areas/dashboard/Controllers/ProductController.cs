using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
