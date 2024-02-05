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
                User = User,
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
                var user = await userManager.FindByIdAsync(model.User.Id);
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
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(IdentityUser model)
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
        public IActionResult UserDetails()
        {
            return View();
        }

    }
}
