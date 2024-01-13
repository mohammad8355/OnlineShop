using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.ViewModels
{
    public class AddEditHeadCategoryViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string IdentifierName { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
