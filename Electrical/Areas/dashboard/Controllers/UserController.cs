using BusinessLogicLayer.CommentService;
using BusinessLogicLayer.NotificationLogic;
using BusinessLogicLayer.OrderService;
using BusinessLogicLayer.TicketService;
using DataAccessLayer.Models;
using Infrustructure.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Models.ViewModels;
using Utility.ReturnMultipleData;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly OrderLogic _orderLogic;
        private readonly CommentLogic _commentLogic;
        private readonly TicketLogic _ticketLogic;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IHubContext<HubConfig> _hubContext;
        private readonly NotificationLogic _notificationLogic;
        public UserController(NotificationLogic notificationLogic,IHubContext<HubConfig> hubContext,TicketLogic ticketLogic,CommentLogic commentLogic,OrderLogic orderLogic,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            _orderLogic = orderLogic;
            _notificationLogic = notificationLogic;
            _commentLogic = commentLogic;
            _ticketLogic = ticketLogic;
            _hubContext = hubContext;
        }
        public async Task<IActionResult> Index()
        {
            var users = userManager.Users.ToList();
            var model = new List<UserListViewModel>();
            foreach (var user in users)
            {
                var role = await userManager.GetRolesAsync(user);
                var modelItem = new UserListViewModel()
                {
                    User = user,
                    Roles = role.ToList(),
                };
                model.Add(modelItem);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            var model = new AddUserViewModel()
            {
                RoleList = new SelectList(roleManager.Roles.ToList(), "Name", "Name"),
                Role = new List<string>(),
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel model)
        {
            model.RoleList = new SelectList(roleManager.Roles.ToList(), "Name", "Name");
            ModelState.Remove("RoleList");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email,
                    EmailConfirmed = model.ComfrimEmail,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = model.ConfrimPhoneNumber,
                    IsEnable = model.IsEnable,
                    brithDate =  model.birthDate,
                };
                var result = await userManager.CreateAsync(user, model.password);
                if (result.Succeeded)
                {
                    foreach (var roleName in model.Role)
                    {

                        result = await userManager.AddToRoleAsync(user, roleName);
                    }
                    if (result.Succeeded)
                    {
                        ViewBag.success = "کاربر با موفقیت افزوده شد";
                        var newmodel = new AddUserViewModel()
                        {
                            RoleList = new SelectList(roleManager.Roles.ToList(), "Name", "Name"),
                            Role = new List<string>(),
                        };
                        return View(newmodel);
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            return View(model);
        }
        [HttpGet]

        public async Task<IActionResult> EditUser(string Id)
        {
            var User = await userManager.FindByIdAsync(Id);
            var roles = await userManager.GetRolesAsync(User);
            var model = new EditUserViewModel()
            {
                RoleList = new SelectList(roleManager.Roles.ToList(), "Name", "Name"),
                Role = roles.ToList(),
                UserName = User.UserName,
                FullName = User.FullName,
                birthDate = User.brithDate,
                Email = User.Email,
                ComfrimEmail = User.EmailConfirmed,
                PhoneNumber = User.PhoneNumber,
                ConfrimPhoneNumber = User.PhoneNumberConfirmed,
                IsEnable = User.IsEnable,

            };
            return View(model);
        }
        [HttpPost]

        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            //model.RoleList = new SelectList(roleManager.Roles.ToList(), "Name", "Name");
            ModelState.Remove("RoleList");
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.EmailConfirmed = model.ComfrimEmail;
                user.PhoneNumber = model.PhoneNumber;
                user.PhoneNumberConfirmed = model.ConfrimPhoneNumber;
                user.IsEnable = model.IsEnable;
                user.FullName = model.FullName;
                user.brithDate = model.birthDate;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    await userManager.RemoveFromRolesAsync(user, roles);
                    foreach (var roleName in model.Role)
                    {
                        result = await userManager.AddToRoleAsync(user, roleName);
                    }
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            return View(model);
        }
        public IActionResult RoleList()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                if (!roleManager.RoleExistsAsync(model.Name).Result)
                {
                    var result = await roleManager.CreateAsync(model);
                    if (result.Succeeded)
                    {
                        ViewBag.success = "افزودن نقش با موفقیت انحام شد";
                        return View();
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        return View(model);
                    }
                };
                ModelState.AddModelError("", "این نقش هم اکنون وجود دارد");
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            return View("AddRole", role);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(model.Id);
                if (role != null)
                {
                    role.Name = model.Name;
                    var result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("RoleList");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        return View("AddRole", model);
                    }

                }
                ModelState.AddModelError("", "خطا");
                return View(model);
            };
            return View("AddRole", model);
        }
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(ApplicationUser model)
        {
            var id = model.Id;
            var user = await userManager.FindByIdAsync(id);
            if(user !=  null)
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            else
            {
                return View(model);
            }
        }
        public async Task<IActionResult> UserDetails(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            var roles = await userManager.GetRolesAsync(user);
            var orders = _orderLogic.AllUserOrder(user.Id);
            var comments = _commentLogic.CommentsOfUser(user.Id);
            var tickets = _ticketLogic.TicketListOfUser(user.Id);
            user.Order = orders;
            user.Commnets = comments;
            user.Tickets = tickets;
            var model = new UserDetailsViewModel()
            {
                User = user,
                Roles = roles.ToList(),
            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            if(role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                return Json(new { name = role.Name, result = result });
            }
            return RedirectToAction("RoleList");
        }
        public async Task<IActionResult> ActiveUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if(user != null)
            if (user.IsEnable)
            {
                user.IsEnable = false;
               var result = await userManager.UpdateAsync(user);
                return Json(new { res = result });
            }
            else
            {
                user.IsEnable = true;
                var result = await userManager.UpdateAsync(user);
                return Json(new { res = result });
            }
            return Json(new { res = false });
        }
        [HttpGet]
        public IActionResult Notification()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Notification(Notification model)
        {
            model.Date = DateTime.Now;
            model.Status = "unread";
          //var result =  await  _notificationLogic.AddNotification(model);
            //if (result)
            //{
               await  _hubContext.Clients.All.SendAsync("receiveMessage", model.message);
            //}
            return View();
        }
    }
}
