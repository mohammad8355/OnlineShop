using BusinessEntity;


namespace PresentationLayer.Models.ViewModels
{
    public class AddEditProductViewModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
        public IEnumerable<IFormFile> File { get; set; }
    }
}
