using Microsoft.AspNetCore.Identity;

namespace PresentationLayer.Models.ViewModels
{
    public class UserListViewModel
    {
        public IdentityUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
