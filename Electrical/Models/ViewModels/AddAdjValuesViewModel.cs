using BusinessEntity;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.ViewModels
{
    public class AddAdjValuesViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(300)]
        public string Value { get; set; }
        [Required]
        public int adjkey_Id { get; set; }
    }
}
