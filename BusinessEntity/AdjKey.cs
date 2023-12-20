using BusinessEntity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class AdjKey
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string DataType { get; set; }

        //navigation property
        public ICollection<AdjValue> adjValues { get; set; }
        public ICollection<KeyToProduct> KeyToProducts { get; set; }
        public ICollection<KeyToSubCategory> keyToSubCategories { get; set; }
    }
}
