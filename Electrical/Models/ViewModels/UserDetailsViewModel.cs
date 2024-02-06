using Microsoft.AspNetCore.Identity;

namespace PresentationLayer.Models.ViewModels
{
    public class UserDetailsViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }

    }

}
