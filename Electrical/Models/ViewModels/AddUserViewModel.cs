using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.ViewModels
{
    public class AddUserViewModel
    {
        public IdentityUser User { get; set; }
        public IEnumerable<string> Role { get; set; }
        public SelectList RoleList { get; set; }
        public bool RememberMe { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Compare("password")]
        [DataType(DataType.Password)]
        public string  ConfrimPassword { get; set; }
    }
}
