using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace PresentationLayer.Models.ViewModels
{
    public class UserListViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
