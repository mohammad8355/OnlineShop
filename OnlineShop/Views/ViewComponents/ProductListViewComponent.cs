
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BusinessLogicLayer.CategoryService;
using BusinessLogicLayer.ProductService;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Views.ViewComponents
{
    public class ProductListViewComponent : ViewComponent
    {
        private readonly ProductLogic _productService;
        private readonly CategoryLogic _categoryService;

        public ProductListViewComponent(ProductLogic productService, CategoryLogic categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? categoryId = null)
        {
            var categories = await _categoryService.GetCategoryNameAndIdList();
            var products = await _productService.GetProductByCategoryIdList(categoryId != null ? categoryId.Value : 0);

            var model = new ProductListByCategoryViewModel
            {
                _categories = categories,
                Products = products,
                SelectedCategoryId = categoryId
            };

            return View(model);
        }
    }
}

