using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public interface IApplicationUser
    {
        public string Id { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<Commnet> Commnets { get; set; }
    }
}
