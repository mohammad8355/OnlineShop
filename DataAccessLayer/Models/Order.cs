
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateCreate { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public int TotalCount { get; set; }
        public bool IsFinally { get; set; }

        //navigation property
        public ICollection<OrderDetails> orderDetails { get; set; }

    }
}
