using DataAccessLayer.Models;

namespace PresentationLayer.Models.ViewModels
{
    public class OrderFinallyViewModel
    {
        public ApplicationUser User { get; set; }
        public Order Order { get; set; }
    }
}
