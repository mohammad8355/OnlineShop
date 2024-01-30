using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class ValueToProduct
    {
        [Key]
        public int Value_Id { get; set; }
        [Key]
        public int Product_Id { get; set; }
        public AdjValue Value { get; set; }
        public Product Product { get; set; }
    }

}
