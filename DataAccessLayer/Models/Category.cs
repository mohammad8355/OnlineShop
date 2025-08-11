using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        //navigation property
        public Category ParentCategory { get; set; }
        public string Cover { get; set; }
        public int? ParentId { get; set; }
        public ICollection<KeyToSubCategory> keyToSubCategories { get; set; }
        public ICollection<CategoryToProduct> CategoryToProducts { get; set; }
        public ICollection<Category> ChildCategories { get; set; }
    }

}
