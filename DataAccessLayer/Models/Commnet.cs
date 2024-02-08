using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Commnet
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Title { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public DateTime LastUpdate { get; set; }
        public bool IsHide { get; set; }
        //navigation property
        public string User_Id { get; set; }
        public ApplicationUser User { get; set; }
        public int? Ticket_Id { get; set; }
        public Ticket Ticket { get; set; }
        //reply comment
        public int? Reply_Id { get; set; }
        public Commnet reply { get; set; }

    }
}
