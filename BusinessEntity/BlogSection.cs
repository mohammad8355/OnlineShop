using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class BlogSection
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(800)]
        public string Text { get; set; }
        [AllowNull]
        [MaxLength(300)]
        public string photo { get; set; }

        //navication property
        public Weblog Weblog { get; set; }
        public int Weblog_Id  { get; set; }
    }
}
