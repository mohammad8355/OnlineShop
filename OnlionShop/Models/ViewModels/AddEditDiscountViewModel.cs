using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PresentationLayer.Models.ViewModels
{
    public class AddEditDiscountViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public decimal Value { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        [MaxLength(100)]
        public string DateBase { get; set; }
        public bool Active { get; set; }
        [MinLength(7)]
        [MaxLength(10)]
        [AllowNull]
        public string DiscountCode { get; set; }
        public List<int> discountToProducts { get; set; }
        public SelectList selectLists { get; set; }
    }
}
