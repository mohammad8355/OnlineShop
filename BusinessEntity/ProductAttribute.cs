using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Models
{
    public class ProductAttribute
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string KeyName { get; set; }
        [Required]
        [MaxLength(150)]
        public string KeyValue { get; set; }
        [Required]
        [MaxLength(150)]
        public string Unit { get; set; }

    }
}
