using BusinessLogicLayer.BlogPostService;
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
        private readonly BlogPostLogic blogPostLogic;
        public HomeController(ILogger<HomeController> logger, ProductLogic productLogic, GeneralLogic generalLogic, BlogPostLogic blogPostLogic)
        {
            _logger = logger;
            this.productLogic = productLogic;
            this.blogPostLogic = blogPostLogic;
            this.generalLogic = generalLogic;
        }

        public async Task<IActionResult> Index()
        {
            var offProducts = await productLogic.OffProducts();
            var PopularProduct = await productLogic.PopularProduct();
            var newestProduct =  productLogic.NewsetProduct();
            var posts = blogPostLogic.blogPostList();
            var aboutus = await generalLogic.ReturnByLabel("aboutus");
            var slider = generalLogic.GeneralList().Where(s => s.label == "slider").ToList();
            var model = new HomePageViewModel()
            {
                HotDiscountProduct = offProducts.ToList(),
                PopularProduct = PopularProduct.ToList(),
                NewsetProduct = newestProduct.ToList(),
                Posts = posts,
                Sliders = slider,
                Aboutus = aboutus.First(),

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
            return View();
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