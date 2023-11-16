using Electrical.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Electrical.Controllers
{
    public class AccountController : Controller
    {
        [Route("Register")]
        [HttpGet]
       public IActionResult Registere()
        {
            return View();
        }
        [Route("Register")]
        [HttpPost]
        public IActionResult Registere(RegisterViewModel model)
        {
            return View();
        }
        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [Route("Login")]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            return View();
        }
        [Route("ForgetPassword")]
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [Route("ForgetPassword")]
        [HttpPost]
        public IActionResult ForgetPassword(ForgetPasswordViewModel model)
        {
            return View();
        }
        [Route("ChangePassword")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Route("ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            return View();
        }
        [Route("ConfrimEmail")]
        public IActionResult ConfrimEmail(int Id,string token)
        {
            return View();
        }
    }
}
