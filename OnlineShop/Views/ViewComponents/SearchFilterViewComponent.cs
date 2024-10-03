using BusinessLogicLayer.AdjKeyService;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Views.ViewComponents
{
    public class SearchFilterViewComponent: ViewComponent
    {
        private readonly AdjKeyLogic _adjkeyLogic;
        public SearchFilterViewComponent(AdjKeyLogic adjkeyLogic)
        {
            _adjkeyLogic = adjkeyLogic;
        }
        public async Task<IViewComponentResult> InvokeAsync(int category_Id)
        {
            var model = new SearchFilterViewModel()
            {
                keys = _adjkeyLogic.AdjKeyListByCategory(category_Id),
                Category_Id = category_Id,
            };
            return View(model);
        }
    }
}
