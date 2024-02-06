using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class TagToBlogPost
    {
        public int Tag_Id { get; set; }
        public int BlogPost_Id { get; set; }
        public Tag Tag { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
