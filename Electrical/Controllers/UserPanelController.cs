using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Controllers
{
    public class UserPanelController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManaManager;
        public UserPanelController(UserManager<ApplicationUser> _userManaManager)
        {
            this._userManaManager = _userManaManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await  _userManaManager.FindByNameAsync(User.Identity.Name);
            var model = new ConsoleUserViewModel()
            {
                User = user,
            };
            return View(model);
        }
        public IActionResult notification()
        {
            return View();
        }
        public IActionResult Account()
        {
            return View();
        }
        public IActionResult changeProfileImage()
        {
            return View();
        }
    }
}
