using BusinessEntity;

namespace PresentationLayer.Models.ViewModels
{
    public class ProductDetailViewModel
    {
        public Product product { get; set; }
        public IEnumerable<Category> categories { get; set; }
    }
}
