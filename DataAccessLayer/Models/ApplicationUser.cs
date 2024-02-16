using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime brithDate { get; set; }
        public bool IsEnable { get; set; }
        [MaxLength(800)]
        public string Address { get; set; }
        [MaxLength(10)]
        [MinLength(10)]
        public string PostalCode { get; set; }
        [MaxLength(20)]
        [MinLength(3)]
        public string city { get; set; }
        [MaxLength(50)]
        public string ProfileImageName { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<Commnet> Commnets { get; set; }
    }
}
