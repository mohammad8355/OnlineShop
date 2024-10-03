using BusinessLogicLayer.BlogPostService;
using BusinessLogicLayer.CommentService;
using BusinessLogicLayer.ContactService;
using BusinessLogicLayer.GeneralService;
using BusinessLogicLayer.ProductService;
using DataAccessLayer.Models;
using Electrical.Models;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ViewModels;
using System.Diagnostics;

namespace PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductLogic productLogic;
        private readonly GeneralLogic generalLogic;
        private readonly ContactsLogic contactsLogic;
        private readonly BlogPostLogic blogPostLogic;
        private readonly CommentLogic _commentLogic;
        public HomeController(CommentLogic commentLogic,ILogger<HomeController> logger, ContactsLogic contactsLogic, ProductLogic productLogic, GeneralLogic generalLogic, BlogPostLogic blogPostLogic)
        {
            _logger = logger;
            this.productLogic = productLogic;
            this.blogPostLogic = blogPostLogic;
            this.contactsLogic = contactsLogic;
            this.generalLogic = generalLogic;
            _commentLogic = commentLogic;
        }

        public async Task<IActionResult> Index()
        {
            var offProducts = await productLogic.OffProducts();
            var PopularProduct = await productLogic.PopularProduct();
            var newestProduct =  productLogic.NewsetProduct();
            var posts = blogPostLogic.blogPostList();
            var aboutus = await generalLogic.ReturnByLabel("aboutus");
            var slider = generalLogic.GeneralList().Where(s => s.label == "slider").ToList();
            var comments = _commentLogic.LimitedComment();
            var model = new HomePageViewModel()
            {
                HotDiscountProduct = offProducts.ToList(),
                PopularProduct = PopularProduct.ToList(),
                NewsetProduct = newestProduct.ToList(),
                Posts = posts,
                Sliders = slider,
                Aboutus = aboutus.First(),
                Comments = comments,

            };
            return View(model);
        }
        public async Task<IActionResult> Aboutus()
        {
            var aboutus = await generalLogic.ReturnByLabel("aboutus");
            return View(aboutus);
        }
        public IActionResult Contactus()
        {
            var model = contactsLogic.ContactList();
            return View(model);
        }
        public IActionResult CommonQuestion()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}