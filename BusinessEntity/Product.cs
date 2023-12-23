﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Models
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
        [Required]
        [MaxLength(300)]
        public string photo { get; set; }
        [AllowNull]
        public int Weight { get; set; }
        [AllowNull]
        public int Width { get; set; }
        [AllowNull]
        public int height { get; set; }
        [AllowNull]
        public int length { get; set; }

        //navigation property
        public SubCategory subCategory { get; set; }
        public int SubCategory_Id { get; set; }
        public ICollection<KeyToProduct>  keyToProducts { get; set; }
        public ICollection<DiscountToProduct> discountToProducts { get; set; }
    }

}
