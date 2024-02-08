using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        [MaxLength(30)]
        public string Status { get; set; }
        [Required]
        public DateTime LastUpdate { get; set; }

        //navigation property
        [MaxLength(128)]
        public string User_Id { get; set; }
        public ApplicationUser User { get; set; }
        public List<Commnet> commnets { get; set; }
        
    }
}
