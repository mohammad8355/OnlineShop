using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class AdjValue
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(300)]
        public string Value { get; set; }

        //navigation property 
        public AdjKey adjKey { get; set; }
        public int adjkey_Id { get; set; }
    }

}
