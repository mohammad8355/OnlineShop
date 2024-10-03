using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.ViewModels
{
    public class AddEditSliderViewModel
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
