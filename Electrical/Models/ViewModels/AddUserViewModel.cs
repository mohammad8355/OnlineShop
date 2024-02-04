using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Models.ViewModels
{
    public class AddUserViewModel
    {
        public IdentityUser User { get; set; }
        public IEnumerable<string> Role { get; set; }
        public SelectList RoleList { get; set; }
        public bool RememberMe { get; set; }
        public string password { get; set; }
        public string  ConfrimPassword { get; set; }
    }
}
