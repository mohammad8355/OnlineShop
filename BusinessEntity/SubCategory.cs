using BusinessEntity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        [MaxLength(100)]
        public string Parent { get; set; }
        [Required]
        [MaxLength(100)]
        public string IdentifierName { get; set; }

        //navigation property
        public Category category { get; set; }
        public int category_Id { get; set; }
        public ICollection<Product> products { get; set; }
        public ICollection<KeyToSubCategory> keyToSubCategories { get; set; }
    }

}
