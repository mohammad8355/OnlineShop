using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("Dashboard")]
    public class DiscountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddDiscount()
        {
            return View();
        }
    }
}
