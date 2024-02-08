
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class DiscountToProduct
    {
        [Key]
        public int Discount_Id { get; set; }
        [Key]
        public int Product_Id { get; set; }

        //navigation property
        public Discount discount { get; set; }
        public Product product { get; set; }
    }
}
