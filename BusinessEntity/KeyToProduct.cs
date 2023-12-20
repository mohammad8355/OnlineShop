using BusinessEntity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class KeyToProduct
    {
        [Key]
        public int Key_Id { get; set; }
        [Key]
        public int Product_Id { get; set; }

        //navigation property
        public AdjKey adjKey { get; set; }
        public Product product { get; set; }
    }
}
