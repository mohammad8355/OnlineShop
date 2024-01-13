using BusinessEntity;
using BusinessLogicLayer.AdjKeyService;
using BusinessLogicLayer.AdjValueService;
using BusinessLogicLayer.CategoryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]

    public class TechnicalSpecificationController : Controller
    {
        private readonly AdjKeyLogic adjKeyLogic;
        private readonly HeadCategoryLogic headCategoryLogic;
        private readonly CategoryLogic categoryLogic;
        private readonly SubCategoryLogic subCategoryLogic;
        private readonly AdjValueLogic adjValueLogic;
        public TechnicalSpecificationController(AdjValueLogic adjValueLogic,CategoryLogic categoryLogic,AdjKeyLogic adjKeyLogic, HeadCategoryLogic headCategoryLogic, SubCategoryLogic subCategoryLogic)
        {
            this.subCategoryLogic = subCategoryLogic;
            this.headCategoryLogic = headCategoryLogic;
            this.adjKeyLogic = adjKeyLogic;
            this.categoryLogic = categoryLogic;
            this.adjValueLogic = adjValueLogic;
        }
        public IActionResult Index()
        {
            var model = adjKeyLogic.AdjKeyList();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddTechnicalSpecification()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTechnicalSpecification(AddEditTechnicalSpecificationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adjkeyModel = new AdjKey()
                {
                    Name = model.Name,
                    DataType = model.DataType,
                };
                var resault = await adjKeyLogic.AddAdjKey(adjkeyModel);
                if (resault)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var categorylist = categoryLogic.CategoryList();
                    ViewBag.categories = new SelectList(categorylist, "Id", "Name");
                    return View(model);
                }
            }
            var categories = categoryLogic.CategoryList();
            ViewBag.categories = new SelectList(categories, "Id", "Name");
            return View(model);
   
        }
        [HttpGet]
        public IActionResult EditTechnicalSpecification(int Id)
        {
            var adjkeyModel = adjKeyLogic.AdjKeyDetail(Id);
            var model = new AddEditTechnicalSpecificationViewModel()
            {
                Id = adjkeyModel.Id,
                Name = adjkeyModel.Name,
                DataType = adjkeyModel.DataType,
            };
            return View("AddTechnicalSpecification", model);
        }
        [HttpPost]
        public async Task<IActionResult> EditTechnicalSpecification(AddEditTechnicalSpecificationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adjKeyModel = adjKeyLogic.AdjKeyDetail(model.Id);
                adjKeyModel.Name = model.Name;
                adjKeyModel.DataType = model.DataType;
                var result = await adjKeyLogic.EditAdjKey(adjKeyModel);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("AddTechnicalSpecification", model);
                }
            }
            return View("AddTechnicalSpecification",model);
        }
        public IActionResult AddValuesToOption()
        {
            return View();
        }
        [HttpGet("TechnicalSpecification/DropDown/Id")]
        public JsonResult DropDown(int category_Id)
        {
            #region make list of categories and subcategories for create select list
            var category = categoryLogic.CategoryDetail(category_Id);
            var subcategories = subCategoryLogic.SubCategoryList().Where(s => s.Parent == category.IdentifierName).ToList();
            #endregion
            return Json(new SelectList(subcategories, "Id", "Name"));
        }
        [HttpGet]
        public async Task<JsonResult> DeleteAdjKey(string Id)
        {
            #region delete product
            int PId = int.Parse(Id);
            var adjkey = adjKeyLogic.AdjKeyDetail(PId);
            var resualt = await adjKeyLogic.DeleteAdjKey(PId);
            #endregion
            return Json(new { name = adjkey.Name, Resualt = resualt });
        }
        [HttpGet]
        public IActionResult AddValues()
        {
            var Adjkeis = adjKeyLogic.AdjKeyList();
            ViewBag.AdjKeys = new SelectList(Adjkeis,"Id","Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddValues(AddAdjValuesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adjvalue = new AdjValue();
                adjvalue.StringValue = model.StringValue;
                adjvalue.NumericValue = model.NumericValue;
                adjvalue.BoolValue = model.BoolValue;
                adjvalue.adjkey_Id = model.adjkey_Id;
                var result = await adjValueLogic.AddAdjValue(adjvalue);
                if (result)
                {
                    ViewBag.success = "عملیات افزودن مقدار با موفقیت انجام شد !";
                    return View();
                }
                ModelState.AddModelError("", "عملیات افزودن مقدار با شکست مواجه شد !");
                return View(model);
            }
            return View(model);

        }
        public IActionResult GetAdjkeyById(int id)
        {
            var adjkey = adjKeyLogic.AdjKeyDetail(id);
            return Json(new { datatype = adjkey.DataType });
        }

    }
}
