using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string message { get; set; }
        public string Status  { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public string User_Id { get; set; }
        public ApplicationUser User { get; set; }

    }
}
