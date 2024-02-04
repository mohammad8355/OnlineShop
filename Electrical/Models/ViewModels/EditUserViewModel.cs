using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Models.ViewModels
{
    public class EditUserViewModel
    {
        public IdentityUser User { get; set; }
        public List<string> Role { get; set; }
        public SelectList RoleList { get; set; }
    }
}
