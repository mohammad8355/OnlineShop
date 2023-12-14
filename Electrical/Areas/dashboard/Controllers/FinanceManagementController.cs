using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("Dashboard")]
    public class FinanceManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
