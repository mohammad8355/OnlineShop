using DataAccessLayer.Models;

namespace PresentationLayer.Models.ViewModels
{
    public class VerificationPaymentViewModel
    {
        public ApplicationUser User { get; set; }
        public Order order { get; set; }
        public string Status { get; set; }
        public string TrackingCode { get; set; }
    }
}
