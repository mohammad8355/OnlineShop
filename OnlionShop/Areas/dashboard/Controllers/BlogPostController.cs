using DataAccessLayer.Models;
using BusinessLogicLayer.BlogPostService;
using BusinessLogicLayer.TagService;
using BusinessLogicLayer.TagToBlogPostService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    [Authorize(Roles = "Admin,Writer")]
    public class BlogPostController : Controller
    {
        private readonly BlogPostLogic blogPostLogic;
        private readonly TagToBlogPostLogic tagToBlogPostLogic;
        private readonly TagLogic tagLogic;
        public BlogPostController(BlogPostLogic blogPostLogic, TagToBlogPostLogic tagToBlogPostLogic, TagLogic tagLogic)
        {
            this.tagToBlogPostLogic = tagToBlogPostLogic;
            this.tagLogic = tagLogic;
            this.blogPostLogic = blogPostLogic;
        }
        public IActionResult Index()
        {
            var model = blogPostLogic.blogPostList();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddBlogPost()
        {
            var tags = tagLogic.TagList();
            ViewBag.tags = new SelectList(tags,"TagName","TagName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBlogPost(AddEditBlogPostViewModel model)
        {
            var blogpost = new BlogPost()
            {
                Title = model.Title,
                Content = model.Content.ToString(),
                CoverLink = model.CoverLink,
                LastUpdates = DateTime.Now,
                PublishDate = DateTime.Now,
                Author = model.Author,
                ReadingTime = model.ReadingTime,
            };
            if (ModelState.IsValid)
            {
                var result = await blogPostLogic.AddBlogPost(blogpost);
                if (result)
                {
                    var blogPost = blogPostLogic.blogPostList().Where(bp => bp.Title == model.Title && bp.Content == model.Content).FirstOrDefault();
                    foreach(var tag in model.Tags)
                    {
                        var tagmodel = new Tag()
                        {
                            TagName = tag,
                        };
                       await tagLogic.AddTag(tagmodel);
                    };
                    foreach(var tag in model.Tags)
                    {
                        var tagModel = tagLogic.TagDetail(tag);
                        var tagtoblogpostModel = new TagToBlogPost()
                        {
                            Tag_Id = tagModel.Id,
                            BlogPost_Id = blogPost.Id,
                        };
                       await tagToBlogPostLogic.AddTagToBlogPost(tagtoblogpostModel);
                    }
                    ViewBag.success = "افزودن پست بلاگ";
                    return View();
                }
                ModelState.AddModelError("", "لطفا تمامی فیلد ها را پر کتید");
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult EditBlogPost(int Id)
        {
            var blogpost = blogPostLogic.BlogPostDetail(Id);
            var tags = tagToBlogPostLogic.TagToBlogPostList().Where(tb => tb.BlogPost_Id == Id).Select(t => t.Tag).ToList();
            var model = new AddEditBlogPostViewModel()
            {
                Title = blogpost.Title,
                Content = blogpost.Content.ToString(),
                CoverLink = blogpost.CoverLink,
                Author = blogpost.Author,
                ReadingTime = blogpost.ReadingTime,
                Tags = tags.Select(t => t.TagName).ToList(),
            };
            var taglist = tagLogic.TagList();
            ViewBag.tags = new SelectList(taglist, "TagName", "TagName");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditBlogPost(AddEditBlogPostViewModel model)
        {
            var blogpost = blogPostLogic.BlogPostDetail(model.Id);
            blogpost.Title = model.Title;
            blogpost.Content = model.Content.ToString();
            blogpost.CoverLink = model.CoverLink;
            blogpost.LastUpdates = DateTime.Now;
            blogpost.Author = model.Author;
            blogpost.ReadingTime = model.ReadingTime;
            
            if (ModelState.IsValid)
            {
                var result = blogPostLogic.EditBlogPost(blogpost);
                if (result)
                {
                    var tags = tagToBlogPostLogic.TagToBlogPostList().Where(tb => tb.BlogPost_Id == model.Id).Select(t => t.Tag).ToList();
                    foreach (var tag in tags)
                    {
                        await tagToBlogPostLogic.DeleteTagToBlogPost(model.Id, tag.Id);
                    }
                    foreach (var tag in model.Tags)
                    {
                        var tagmodel = new Tag()
                        {
                            TagName = tag,
                        };
                        await tagLogic.AddTag(tagmodel);
                    };
                    foreach (var tag in model.Tags)
                    {
                        var tagModel = tagLogic.TagDetail(tag);
                        var tagtoblogpostModel = new TagToBlogPost()
                        {
                            Tag_Id = tagModel.Id,
                            BlogPost_Id = model.Id,
                        };
                        await tagToBlogPostLogic.AddTagToBlogPost(tagtoblogpostModel);
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "لطفا تمامی فیلد ها را پر کتید");
                return View(model);
            }
            return View(model);
        }
        public async Task<IActionResult> DeleteBlogPost(int Id)
        {
            var blogpost = blogPostLogic.BlogPostDetail(Id);
            var result = await blogPostLogic.DeleteBlogPost(Id);
            return Json(new { name = blogpost.Title, result = result });
        }
    }
}
