using BusinessEntity;

namespace PresentationLayer.Models.ViewModels
{
    public class AddEditSubCategoryViewModel
    {
        public ICollection<Category> categories { get; set; }
        public SubCategory subCategory { get; set; }
    }
}
