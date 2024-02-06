using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.ViewModels
{
    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        public string? FullName { get; set; }
        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MaxLength(15)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public bool ComfrimEmail { get; set; }
        public bool ConfrimPhoneNumber { get; set; }
        public DateTime birthDate { get; set; }
        public bool IsEnable { get; set; }
        public IEnumerable<string> Role { get; set; }
        public SelectList RoleList { get; set; }
 
    }
}
