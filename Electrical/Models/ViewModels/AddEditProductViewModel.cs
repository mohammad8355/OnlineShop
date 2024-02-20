using DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.ViewModels
{
    public class AddEditProductViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public int QuantityInStock { get; set; }
        public int Weight { get; set; }
        public int Width { get; set; }
        public int height { get; set; }
        public int length { get; set; }
        [Required]
        public IEnumerable<IFormFile> File { get; set; }
        [Required]
        public List<int> SelectList { get; set; }
        public List<Brand> brands { get; set; }
        public int Brand_Id { get; set; }
    }
}
