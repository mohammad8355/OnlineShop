using BusinessLogicLayer.BlogPostService;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogPostLogic blogPostLogic;
        public BlogController(BlogPostLogic blogPostLogic)
        {
            this.blogPostLogic = blogPostLogic;
        }
        public IActionResult Index()
        {
            var model = new BlogPostIndexViewModel()
            {
                Posts = blogPostLogic.blogPostList(),
            };
            return View(model);
        }
        public IActionResult Post(string Title)
        {
            
            var model = new PostViewModel()
            {
                Post = blogPostLogic.BlogPostDetail(Title),
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult SearchPost(string search = "")
        {
            var model = blogPostLogic.SearchPost(search);
            return PartialView("Partials/SearchPostPartial");
        }

    }
}
