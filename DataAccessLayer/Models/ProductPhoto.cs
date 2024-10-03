
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ProductPhoto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string format { get; set; }

        //navigation property
        public int Product_Id { get; set; }
        public Product product { get; set; }
    }
}
