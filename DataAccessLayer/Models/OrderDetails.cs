
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        public Product Product { get; set; }
        [Required]
        public int count { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }

        //navigation property
        public Order order { get; set; }
        public int order_Id { get; set; }

    }

}
