using BusinessLogicLayer.CategoryService;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Views.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly CategoryLogic _categoryLogic;
        public SearchViewComponent(CategoryLogic categoryLogic)
        {
            _categoryLogic = categoryLogic;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new SearchViewModel()
            {
                Categories = _categoryLogic.CategoryList().ToList(),
            };
                return View(model);
        }


    }



}

