﻿using BusinessEntity;
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
        public TechnicalSpecificationController(AdjValueLogic adjValueLogic, CategoryLogic categoryLogic, AdjKeyLogic adjKeyLogic, HeadCategoryLogic headCategoryLogic, SubCategoryLogic subCategoryLogic)
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
                    Description = model.Description,
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
                Description = adjkeyModel.Description,
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
                adjKeyModel.Description = model.Description;
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
            return View("AddTechnicalSpecification", model);
        }
        public IActionResult AddValuesToOption()
        {
            return View();
        }
        [HttpGet("TechnicalSpecification/DropDown/adjkey_Id")]
        public IActionResult DropDown(int adjkey_Id)
        {
            if (adjkey_Id != 0)
            {
                #region make list of adjkey and adjvalues for create select list
                var adjkey = adjKeyLogic.AdjKeyDetail(adjkey_Id);
                var adjvalues = adjValueLogic.AdjValueList().Where(s => s.adjkey_Id == adjkey.Id).ToList();
                #endregion
                return Json(new SelectList(adjvalues, "Id", "Value"));
            }
            return RedirectToAction("EditValue");
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
            ViewBag.AdjKeys = new SelectList(Adjkeis, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddValues(AddAdjValuesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adjvalue = new AdjValue();
                adjvalue.Value = model.Value;
                adjvalue.adjkey_Id = model.adjkey_Id;
                var result = await adjValueLogic.AddAdjValue(adjvalue);
                if (result)
                {
                    var Adjkeis = adjKeyLogic.AdjKeyList();
                    ViewBag.AdjKeys = new SelectList(Adjkeis, "Id", "Name");
                    ViewBag.success = "عملیات افزودن مقدار با موفقیت انجام شد !";
                    return View();
                }
                ModelState.AddModelError("", "عملیات افزودن مقدار با شکست مواجه شد !");
                return View(model);
            }
            return View(model);

        }
        [HttpGet]
        public IActionResult EditValue(int Id)
        {
            if (Id != 0)
            {
                var adjvalue = adjValueLogic.AdjValueDetail(Id);
                var model = new AddAdjValuesViewModel()
                {
                    Id = adjvalue.Id,
                    Value = adjvalue.Value,
                    adjkey_Id = adjvalue.adjkey_Id,
                };
                return Json(new { model = model });
            }
            var Adjkeis = adjKeyLogic.AdjKeyList();
            ViewBag.AdjKeys = new SelectList(Adjkeis, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditValue(AddAdjValuesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var adjvalue = adjValueLogic.AdjValueDetail(model.Id);
                adjvalue.Value = model.Value;
                adjvalue.adjkey_Id = model.adjkey_Id;
                adjvalue.Id = model.Id;
                var result = await adjValueLogic.EditAdjValue(adjvalue);
                if (result)
                {
                    var Adjkeis = adjKeyLogic.AdjKeyList();
                    ViewBag.AdjKeys = new SelectList(Adjkeis, "Id", "Name");
                    ViewBag.success = "عملیات افزودن مقدار با موفقیت انجام شد !";
                    return View();
                }
                ModelState.AddModelError("", "عملیات ویرایش با شکست مواجه شد");
                return View(model);
            }
            return View(model);
        }
        public IActionResult GetAdjkeyById(int id)
        {
            if (id != 0)
            {
                var adjkey = adjKeyLogic.AdjKeyDetail(id);
                return Json(new { Description = adjkey.Description });
            }
            return Json(new { Description = "" });
        }
        public async Task<IActionResult> DeleteValue(int Id, int key_Id)
        {
            var adjkey = adjKeyLogic.AdjKeyDetail(key_Id);
            var adjvalue = adjValueLogic.AdjValueDetail(Id);
            var result = await adjValueLogic.DeleteAdjValue(Id);
            if (result)
            {
                return Json(new { name = adjvalue.Value, result = result });
                    
            }
          return Json(new { name = ' ', result = result });
        }

    }
}
