using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class General
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [MaxLength(200)]
        public string Link { get; set; }
        [MaxLength(100)]
        public string label { get; set; }
        [MaxLength(200)]
        public string ImageName { get; set; }
    }

}
