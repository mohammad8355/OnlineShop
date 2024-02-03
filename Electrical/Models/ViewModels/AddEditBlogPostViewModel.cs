using System.ComponentModel.DataAnnotations;
namespace PresentationLayer.Models.ViewModels
{
    public class AddEditBlogPostViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public int ReadingTime { get; set; }
        [Required]
        [MaxLength(300)]
        public string CoverLink { get; set; }
        [Required]
        [MaxLength(50)]
        public string Author { get; set; }
        [Required, MaxLength(2000)]
        public string Content { get; set; }
    }
}
