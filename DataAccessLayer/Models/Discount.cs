using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Discount
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
        public string DiscountCode { get; set; }

        //navigation property
        public ICollection<DiscountToProduct> discountToProducts { get; set; }
    }
}
