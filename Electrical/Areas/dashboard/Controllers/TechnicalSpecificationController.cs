using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]

    public class TechnicalSpecificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddTechnicalSpecification()
        {
            return View();
        }
    }
}
