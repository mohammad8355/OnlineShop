using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Views.ViewComponents
{
    public class RelatedProductViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<Product> RelatedProduct)
        {
            var model = new RelatedProductViewModel()
            {
                relatedProduct = RelatedProduct,
            };
            return View(model);
        }
    }
    public class RelatedProductViewModel
    {
        public List<Product> relatedProduct { get; set; }
    }
}
