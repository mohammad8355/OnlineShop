using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Electrical.Models.ViewModels
{
    public class ForgetPasswordViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(300)]
        [DisplayName("ایمیل")]
        public string Email { get; set; }
    }
}
