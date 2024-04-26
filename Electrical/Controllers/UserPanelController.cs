using BusinessLogicLayer.favoriteProductService;
using BusinessLogicLayer.NotificationLogic;
using BusinessLogicLayer.OrderService;
using BusinessLogicLayer.ProductService;
using DataAccessLayer.Models;
using Infrustructure.uploadfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class UserPanelController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManaManager;
        private readonly UploadFile fileManage;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly favoriteProductLogic favoriteProductLogic;
        private readonly OrderLogic _orderLogic;
        private readonly NotificationLogic _notificationLogic;
        public UserPanelController(NotificationLogic notificationLogic,OrderLogic orderLogic, favoriteProductLogic favoriteProductLogic, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> _userManaManager, UploadFile fileManage)
        {
            this._userManaManager = _userManaManager;
            this.fileManage = fileManage;
            _notificationLogic = notificationLogic;
            this.webHostEnvironment = webHostEnvironment;
            this.favoriteProductLogic = favoriteProductLogic;
            _orderLogic = orderLogic;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManaManager.FindByNameAsync(User.Identity.Name);
            var products = await favoriteProductLogic.LikedProducts(user.Id);
            var model = new ConsoleUserViewModel()
            {
                User = user,
                LikedProducts = products.Select(p => p.Product).ToList(),
            };
            return View(model);
        }
        public async Task<IActionResult> notification()
        {
            var currentUser = await _userManaManager.FindByNameAsync(User.Identity.Name);
            var model = await _notificationLogic.UserNotifications(currentUser.Id);
            return View(model);
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
            if (file != null)
            {
                var name = User.Identity.Name;
                var destination = "Image\\Users" + "\\" + name + "\\";
                var webrootpath = webHostEnvironment.WebRootPath;
                var path = Path.Combine(webrootpath, destination);
                var result = await fileManage.Upload(name, path, 0, formats, file);
                if (result.result)
                {
                    var user = await _userManaManager.FindByNameAsync(name);
                    user.ProfileImageName = name + result.message.ToString();
                    var updateUserResult = await _userManaManager.UpdateAsync(user);
                    if (updateUserResult.Succeeded)
                    {
                        return Json(new { message = "پروفایل با موفقیت تغییر کرد" });
                    }
                    return Json(new { message = updateUserResult.Errors.First().Description });
                }
                return Json(new { message = "خطا در سرور !" });
            }
            return Json(new { message = "لطفا یک عکس انتخاب کنید" });
        }
        public IActionResult Orders()
        {

            return View();
        }
        [HttpGet]
        public IActionResult OrderFilter(OrderFilterViewModel model)
        {
            var resultModel = new OrdersViewModel();
            if (model != null)
            {
                resultModel.Orders = _orderLogic.OrderFilter(model.FromDate, model.ToDate, model.Search, model.Status);
            }
            else
            {
                resultModel.Orders = _orderLogic.OrderList();
            }
            return PartialView("Partials/_OrdersPartial", resultModel);
        }
    }
}
