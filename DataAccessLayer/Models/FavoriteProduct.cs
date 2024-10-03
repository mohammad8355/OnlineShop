using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class FavoriteProduct
    {
        public int Product_Id { get; set; }
        public Product Product { get; set; }
        public string User_Id { get; set; }
        public ApplicationUser User { get; set; }
    }
}
