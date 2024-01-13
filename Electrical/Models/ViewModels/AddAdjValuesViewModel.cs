using BusinessEntity;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models.ViewModels
{
    public class AddAdjValuesViewModel
    {
        [Key]
        public int Id { get; set; }
        public int NumericValue { get; set; }
        [MaxLength(300)]
        public string? StringValue { get; set; }
        public bool BoolValue { get; set; }
        public int adjkey_Id { get; set; }
    }
}
