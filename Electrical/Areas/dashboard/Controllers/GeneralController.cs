using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("Dashboard")]
    public class GeneralController : Controller
    {
        public IActionResult Aboutus()
        {
            return View();
        }
        public IActionResult ContactList()
        {
            return View();
        }
        public IActionResult AddContact()
        {
            return View();
        }
        public IActionResult SliderList()
        {
            return View();
        }
        public IActionResult AddSlider()
        {
            return View();
        }
    }
}
