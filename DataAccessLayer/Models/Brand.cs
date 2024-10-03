using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string ImageName { get; set; }
        public float rate { get; set; }

        //navigation 
        public List<Product> products { get; set; }
    }
}
