using BusinessLogicLayer.CategoryService;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Views.ViewComponents
{
    public class CategoryMegaMenuViewComponent:ViewComponent
    {
        private readonly CategoryLogic categoryLogic;
        public CategoryMegaMenuViewComponent(CategoryLogic categoryLogic)
        {
            this.categoryLogic = categoryLogic;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new CategoryMegaMenuViewComponentModel()
            {
                ParentCategories = categoryLogic.CategoryParent(),
            };
            return View(model);
        }
        public class CategoryMegaMenuViewComponentModel
        {
            public List<Category> ParentCategories { get; set; }
            public List<Category> childCategories { get; set; }

        }

    }
}
