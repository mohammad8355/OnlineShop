using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("Dashboard")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddUser()
        {
            return View();  
        }
        public IActionResult EditUser()
        {
            return View();
        }
        public IActionResult RoleList()
        {
            return View();
        }
        public IActionResult AddRole()
        {
            return View();
        }
        public IActionResult UserDetails()
        {
            return View();
        }

    }
}
