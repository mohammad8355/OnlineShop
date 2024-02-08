using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Tag
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string TagName { get; set; }

        //navigation 
        public List<TagToBlogPost> tagToBlogPosts { get; set; }
    }

}
