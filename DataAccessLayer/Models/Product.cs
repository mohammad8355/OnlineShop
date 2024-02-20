using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public int QuantityInStock { get; set; }
        [AllowNull]
        public int Weight { get; set; }
        [AllowNull]
        public int Width { get; set; }
        [AllowNull]
        public int height { get; set; }
        [AllowNull]
        public int length { get; set; }
        public float Discount { get; set; }
        [MaxLength(20)]
        public string ProductCode { get; set; }

        //navigation property
        public ICollection<KeyToProduct>  keyToProducts { get; set; }
        public ICollection<DiscountToProduct> discountToProducts { get; set; }
        public ICollection<CategoryToProduct> CategoryToProducts { get; set; }
        public ICollection<ProductPhoto>  ProductPhotos { get; set; }
        public IEnumerable<ValueToProduct> valueToProducts { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public List<Commnet> commnets { get; set; }
        public Brand brand { get; set; }
        public int Brand_Id { get; set; }
    }

}
