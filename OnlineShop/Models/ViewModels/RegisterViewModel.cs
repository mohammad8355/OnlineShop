using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Electrical.Models.ViewModels
{
    public class RegisterViewModel
    {

        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("نام کاربری")]
        public string UserName { get; set; }
        [Required]
        [MaxLength(300)]
        [DisplayName("ایمیل")]
        public string Email { get; set; }
        [Required]
        [MaxLength(200)]
        [DisplayName("رمز عبور")]
        public string Password { get; set; }
        [Required]
        [MaxLength(200)]
        [DisplayName("تکرار رمز عبور")]
        [Compare("Password")]
        public string ConfrimPassword { get; set; }

    }
}
