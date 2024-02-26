using DataAccessLayer.Models;

namespace PresentationLayer.Models.ViewModels
{
    public class HomePageViewModel
    {
        public List<General> Sliders { get; set; }
        public List<Product> HotDiscountProduct { get; set; }
        public List<Product> NewsetProduct { get; set; }
        public List<Product> PopularProduct { get; set; }
        public List<BlogPost> Posts { get; set; }
        public List<Commnet> Comments { get; set; }
        public General Aboutus  { get; set; }

    }
}
