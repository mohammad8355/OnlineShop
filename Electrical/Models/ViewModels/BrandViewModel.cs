using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.ViewModels
{
    public class BrandViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public IFormFile file { get; set; }
        public float rate { get; set; }
    }
}
