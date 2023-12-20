using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
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
        public DateTime dateTime { get; set; }
        [Required]
        [MaxLength(100)]
        public string DateBase { get; set; }
        public bool Active { get; set; }

        //navigation property
        public ICollection<DiscountToProduct> discountToProducts { get; set; }
    }
}
