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
            return View();
        }
        public IActionResult Post(string Title)
        {
            
            var model = new PostViewModel()
            {
                Post = blogPostLogic.BlogPostDetail(Title),
            };
            return View(model);
        }

    }
}
