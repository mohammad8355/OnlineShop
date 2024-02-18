using BusinessLogicLayer.ProductPhotoService;
using BusinessLogicLayer.ProductService;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductLogic productLogic;
        private readonly ProductPhotoLogic productPhotoLogic;
        public ProductController(ProductLogic productLogic, ProductPhotoLogic productPhotoLogic)
        {
            this.productPhotoLogic = productPhotoLogic;
            this.productLogic = productLogic;
        }
        public IActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ProductShow(int Id)
        {
            var product = await productLogic.ProductDetail(Id);
            var model = new ProductShowViewModel()
            {
                product = product,
            };
            return View(model);
        }
    }
}
