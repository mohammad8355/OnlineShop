
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class KeyToProduct
    {
        [Key]
        public int Key_Id { get; set; }
        [Key]
        public int Product_Id { get; set; }
        public bool IsSpecial { get; set; }

        //navigation property
        public AdjKey adjKey { get; set; }
        public Product product { get; set; }
    }
}
