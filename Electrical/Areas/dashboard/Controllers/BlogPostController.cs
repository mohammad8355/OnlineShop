using BusinessEntity;
using BusinessLogicLayer.BlogPostService;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Areas.dashboard.Controllers
{
    [Area("dashboard")]
    public class BlogPostController : Controller
    {
        private readonly BlogPostLogic blogPostLogic;
        public BlogPostController(BlogPostLogic blogPostLogic)
        {
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
            var model = new AddEditBlogPostViewModel()
            {
                Title = blogpost.Title,
                Content = blogpost.Content,
                CoverLink = blogpost.CoverLink,
                Author = blogpost.Author,
                ReadingTime = blogpost.ReadingTime,
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult EditBlogPost(AddEditBlogPostViewModel model)
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
