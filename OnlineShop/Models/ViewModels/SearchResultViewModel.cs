using DataAccessLayer.Models;

namespace PresentationLayer.Models.ViewModels
{
    public class SearchResultViewModel
    {
        public List<Product> Products { get; set; }
        public int Category_Id { get; set; } = 0;

    }
}
