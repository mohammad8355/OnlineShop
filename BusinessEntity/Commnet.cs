using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class Commnet
    {
        [Key]
        public string Id { get; set; }
        [MaxLength(50)]
        public string? Title { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public DateTime LastUpdate { get; set; }

        //navigation property
        public int User_Id { get; set; }
        public IApplicationUser User { get; set; }
        public int Ticket_Id { get; set; }
        public Ticket Ticket { get; set; }
        //reply comment
        public int Reply_Id { get; set; }
        public Commnet reply { get; set; }

    }
}
