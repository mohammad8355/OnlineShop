using DataAccessLayer.Models;
using BusinessLogicLayer.TagService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    public class TagController : Controller
    {
        private readonly TagLogic tagLogic;
        public TagController(TagLogic tagLogic)
        {
            this.tagLogic = tagLogic;
        }
        public IActionResult Index()
        {
            var taglist = tagLogic.TagList();
            return View(taglist);
        }
        [HttpGet]
        public async Task<IActionResult> AddTag()
        {
            ViewBag.Tags = new SelectList(new List<Tag>(), "TagName", "TagName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTag(AddTagViewModel tags)
        {
            var result = false;
            var tagList = new List<string>();
            foreach (var tag in tags.TagNames)
            {
                var tagModel = new Tag()
                {
                    TagName = tag,
                };
                result = await tagLogic.AddTag(tagModel);
                if (result)
                {
                    tagList.Add(tag);
                }
            }
            ViewBag.success = " افزودن تگ های ";
            foreach(var tagname in tagList)
            {
                ViewBag.success += tagname + ",";
            }
            ViewBag.success += "با موفقیت انجام شد";
            return View();
        }
        [HttpGet]
        public IActionResult EditTag(int Id)
        {
            var tag =  tagLogic.TagDetail(Id);
            return View(tag);
        }
        [HttpPost]
        public IActionResult EditTag(Tag tag)
        {
            var result = tagLogic.EditTag(tag);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View(tag);
        }
        public IActionResult DeleteTag(int Id)
        {
            var tagName = tagLogic.TagDetail(Id).TagName;
            var result = tagLogic.DeleteTag(Id);
            return Json(new { name = tagName, result = result });
        }
    }
}


