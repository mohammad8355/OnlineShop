using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    [Authorize(Roles ="Admin,Supporter,Writer")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
