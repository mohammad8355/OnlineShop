using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class KeyToSubCategory
    {
        [Key]
        public int key_Id { get; set; }
        [Key]
        public int SubCategory_Id { get; set; }

        //navigation proprty
        public AdjKey adjKey { get; set; }
        public SubCategory subCategory { get; set; }
    }
}
