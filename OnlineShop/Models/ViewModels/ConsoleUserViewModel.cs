using DataAccessLayer.Models;

namespace PresentationLayer.Models.ViewModels
{
    public class ConsoleUserViewModel
    {
        public ApplicationUser User { get; set; }
        public List<Product> LikedProducts { get; set; }
    }
}
