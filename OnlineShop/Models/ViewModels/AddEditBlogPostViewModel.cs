using System.ComponentModel.DataAnnotations;
namespace PresentationLayer.Models.ViewModels
{
    public class AddEditBlogPostViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        //[RegularExpression(@"^[^\\/:?""<>|]*$", ErrorMessage = "نمی توانید از این کاراکتر ها در نام محصول استفاه کنید")]
        public string Title { get; set; }
        [Required]
        public int ReadingTime { get; set; }
        [Required]
        [MaxLength(300)]
        public string CoverLink { get; set; }
        [Required]
        [MaxLength(50)]
        public string Author { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public List<string> Tags { get; set; }
    }
}
