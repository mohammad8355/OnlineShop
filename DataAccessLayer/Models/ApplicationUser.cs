using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime brithDate { get; set; }
        public bool IsEnable { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<Commnet> Commnets { get; set; }
    }
}
