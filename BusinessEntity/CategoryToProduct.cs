using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class CategoryToProduct
    {
        [Key]
        public int Category_Id { get; set; }
        [Key]
        public int Product_Id { get; set; }
        public Category Category { get; set; }
        public Product Product { get; set; }
    }
}
