using DataAccessLayer.Models;

namespace PresentationLayer.Models.ViewModels
{
    public class ProductDetailViewModel
    {
        public Product product { get; set; }
        public IEnumerable<Category> categories { get; set; }
        public List<KeyToProduct> keyToProducts { get; set; }
        public List<ValueToProduct> valueToProducts { get; set; }
        public List<Commnet> Comments { get; set; }
    }
}
