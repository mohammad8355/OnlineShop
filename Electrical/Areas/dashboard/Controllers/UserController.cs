using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Models.ViewModels;
using Utility.ReturnMultipleData;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("Dashboard")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;

        }
        public IActionResult Index()
        {
            var model = userManager.Users.ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            var model = new AddUserViewModel()
            {
                RoleList = new SelectList(roleManager.Roles.ToList(), "Id", "Name"),
                Role = new List<string>(),
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await userManager.CreateAsync(model.User, model.password);
                if (result.Succeeded)
                {
                    foreach (var roleName in model.Role)
                    {

                        result = await userManager.AddToRoleAsync(model.User, roleName);
                    }
                    if (result.Succeeded)
                    {
                        ViewBag.success = "کاربر با موفقیت افزوده شد";
                        return View();
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
            };
            return View(User);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.User.Id);
                if (user != null)
                {
                    user.UserName = model.User.UserName;
                    user.Email = model.User.Email;
                    user.EmailConfirmed = model.User.EmailConfirmed;
                    user.PhoneNumber = model.User.PhoneNumber;
                    user.PhoneNumberConfirmed = model.User.PhoneNumberConfirmed;
                    user.LockoutEnabled = model.User.LockoutEnabled;
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var roles = await userManager.GetRolesAsync(model.User);
                        await userManager.RemoveFromRolesAsync(model.User, roles);
                        foreach (var roleName in model.Role)
                        {
                            result = await userManager.AddToRoleAsync(model.User, roleName);
                        }
                        return RedirectToAction("Index");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        return View(model);
                    }

                }
                ModelState.AddModelError("", "کاربری وجود ندارد");
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
            return View("AddRole",role);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                var result = await roleManager.UpdateAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View("AddRole", model);
                }
            };
            return View("AddRole", model);
        }
        public IActionResult UserDetails()
        {
            return View();
        }

    }
}
