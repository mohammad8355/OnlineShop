using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Electrical.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        [DisplayName("رمز قبلی")] 
        public string OldPassword { get; set; }
        [Required]
        [MaxLength(200)]
        [DisplayName("رمز جدید")]
        public string NewPassword { get; set; }
        [Required]
        [MaxLength(200)]
        [DisplayName("تکرار رمز عبور")]
        [Compare("NewPassword")]
        public string ConfrimPassword { get; set; }

    }
}
