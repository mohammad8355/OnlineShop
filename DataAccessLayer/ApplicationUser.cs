using Microsoft.AspNetCore.Identity;

namespace PresentationLayer.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public DateTime brithDate { get; set; }
        public  bool IsEnable { get; set; }


    }
}
