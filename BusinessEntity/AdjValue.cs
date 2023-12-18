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
        [AllowNull]
        public int NumericValue { get; set; }
        [AllowNull]
        [MaxLength(300)]
        public string StringValue { get; set; }
        [AllowNull]
        public bool BoolValue { get; set; }
    }

}
