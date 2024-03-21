using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        public DateTime PublishDate { get; set; }
        [Required]
        public DateTime LastUpdates { get; set; }
        [Required]
        public  int ReadingTime { get; set; }
        [Required]
        [MaxLength(300)]
        public  string CoverLink { get; set; }
        [Required]
        [MaxLength(50)]
        public string Author { get; set; }
        [Required]
        public string Content { get; set; }

        //navigation 
        public List<TagToBlogPost> TagToBlogPosts { get; set; }
        public List<Commnet> Comments { get; set; }
    }
}
