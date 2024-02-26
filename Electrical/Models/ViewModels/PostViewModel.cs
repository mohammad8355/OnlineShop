using DataAccessLayer.Models;

namespace PresentationLayer.Models.ViewModels
{
    public class PostViewModel
    {
        public BlogPost Post { get; set; }
        public List<BlogPost> SidebarPost { get; set; }
    }
}
