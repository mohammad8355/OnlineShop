using BusinessLogicLayer.CategoryService;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace PresentationLayer.Views.ViewComponents
{
    public class CategoryBreadCrumpViewComponent : ViewComponent
    {
        private readonly CategoryLogic categoryLogic;
        public CategoryBreadCrumpViewComponent(CategoryLogic categoryLogic)
        {
            this.categoryLogic = categoryLogic;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            var model = new BreadCrumpModel()
            {
                Category_Id = Id,
                categories = categoryLogic.CategoryList().ToList(),
            };

            return View(model);

        }
        public class BreadCrumpModel
        {
            public List<Category> categories { get; set; }
            public int Category_Id { get; set; }
        }
    }
}
