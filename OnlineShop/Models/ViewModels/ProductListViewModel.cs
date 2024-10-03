using DataAccessLayer.Models;

namespace PresentationLayer.Models.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> products { get; set; }
        public IEnumerable<Category> categories { get; set; }

    }
}
