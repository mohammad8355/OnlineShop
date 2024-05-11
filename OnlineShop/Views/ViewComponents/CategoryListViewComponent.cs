using DataAccessLayer.Models;
using BusinessLogicLayer.CategoryService;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Views.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly CategoryLogic categoryLogic;
        public CategoryListViewComponent(CategoryLogic categoryLogic)
        {
            this.categoryLogic = categoryLogic;
        }
        public async Task<IViewComponentResult> InvokeAsync(string nameparams)
        {
            var model = new CategoryListViewModel()
            {
                categories = categoryLogic.CategoryList().Where(c => c.ParentId == null).ToList(),
                nameParam = nameparams,
            };
            return View(model);
        }
    }
    public class CategoryListViewModel
    {
        public List<Category> categories { get; set; }
        public string nameParam { get; set; }
    }

}
