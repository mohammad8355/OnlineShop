using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.ViewModels
{
    public class BrandViewModel
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(20)]
        [RegularExpression(@"^[^\\/:?""<>|]*$", ErrorMessage = "نمی توانید از این کاراکتر ها در نام محصول استفاه کنید")]
        public string Name { get; set; }
        public IFormFile file { get; set; }
        public float rate { get; set; }
    }
}
