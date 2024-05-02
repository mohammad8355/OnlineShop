using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Utility.ReturnMultipleData;

namespace PresentationLayer.Views.ViewComponents
{
    public class UserProfileViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        public UserProfileViewComponent(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<ApplicationUser> ReturnUser()
        {
            var username = await userManager.FindByNameAsync(User.Identity.Name);
            return username;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await ReturnUser();
            var ProfileImageName = user.ProfileImageName;
            var imageAddress = "";
            if (string.IsNullOrEmpty(ProfileImageName))
            {
                ProfileImageName = "UserProfile.jpg";
              imageAddress = "Image/Users/defaultUser/" + ProfileImageName;
            }
            else
            {
                imageAddress = "Image/Users/" + user.UserName + "\\" + ProfileImageName;
            }
            var model = new ProfileViewModel()
            {
                Username = user.UserName,
                imageaddress = imageAddress,
            };
            return View(model);
        }
        public class ProfileViewModel
        {
            public string Username { get; set; }
            public string imageaddress { get; set; }
        }
    }
}
