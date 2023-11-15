using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Electrical.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("نام کاربری")]
        public string UserName { get; set; }
        [Required]
        [MaxLength(200)]
        [DisplayName("رمز عبور")]
        public string Password { get; set; }
    }
}
