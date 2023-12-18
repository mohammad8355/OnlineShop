using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    public class Weblog : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddWeblog()
        {
            return View();
        }
    }
}
