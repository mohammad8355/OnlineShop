using DataAccessLayer.Models;
using Infrustructure.uploadfile;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Controllers
{
    public class UserPanelController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManaManager;
        private readonly UploadFile fileManage;
        private readonly IWebHostEnvironment webHostEnvironment;
        public UserPanelController(IWebHostEnvironment webHostEnvironment,UserManager<ApplicationUser> _userManaManager, UploadFile fileManage)
        {
            this._userManaManager = _userManaManager;
            this.fileManage = fileManage;
            this.webHostEnvironment = webHostEnvironment;
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
        [HttpPost]
        public async Task<IActionResult> changeProfileImage(IFormFile file)
        {
            var formats = new List<string>()
            {
                ".jpg",
                ".jpeg",
                ".jfif",
                ".png",
            };
            if(file != null)
            {
                var name = User.Identity.Name;
                var destination = "Image\\Users" + "\\" + name  + "\\";
                var webrootpath = webHostEnvironment.WebRootPath;
                var path = Path.Combine(webrootpath, destination);
                var result = await fileManage.Upload(name, path, 0, formats, file);
                if ((bool)result.First())
                {
                    var user = await _userManaManager.FindByNameAsync(name);
                    user.ProfileImageName = name + result.Last().ToString();
                    var updateUserResult = await _userManaManager.UpdateAsync(user);
                    if (updateUserResult.Succeeded)
                    {
                        return Json(new { message = "پروفایل با موفقیت تغییر کرد" });
                    }
                    return Json(new { message = updateUserResult.Errors.First().Description });
                }
                return Json(new {  message = "خطا در سرور !" });
            }
            return Json(new {  message = "لطفا یک عکس انتخاب کنید"});
        }
    }
}
