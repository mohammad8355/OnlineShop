using BusinessLogicLayer.CategoryService.Dtos;
using BusinessLogicLayer.ProductService.Dtos;
using DataAccessLayer.Models;

namespace PresentationLayer.Models.ViewModels
{
    public class HomePageViewModel
    {
        public List<General> Sliders { get; set; }
        public List<CategoryCoverListDto> CategoryList { get; set; }
        public List<ProductCardDto> HotDiscountProduct { get; set; }
        public List<ProductCardDto> NewsetProduct { get; set; }
        public List<ProductCardDto> PopularProduct { get; set; }
        public List<BlogPost> Posts { get; set; }
        public List<Commnet> Comments { get; set; }
        public General Aboutus  { get; set; }

    }
}
