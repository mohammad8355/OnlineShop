using DataAccessLayer.Models;

namespace PresentationLayer.Models.ViewModels
{
    public class ProductShowViewModel
    {
        public Product product { get; set; }
        public List<AdjKey> specialkeys { get; set; }

    }
}
