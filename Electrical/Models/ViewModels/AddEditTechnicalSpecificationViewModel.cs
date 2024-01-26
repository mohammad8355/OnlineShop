using BusinessEntity;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.ViewModels
{
    public class AddEditTechnicalSpecificationViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        [Required]
        public List<int> subCategories { get; set; }
    }
}
