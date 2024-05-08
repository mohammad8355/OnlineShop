using BusinessLogicLayer.favoriteProductService;
using BusinessLogicLayer.KeyToProductService;
using BusinessLogicLayer.ProductPhotoService;
using BusinessLogicLayer.ProductService;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.ViewModels;

namespace PresentationLayer.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductLogic productLogic;
        private readonly ProductPhotoLogic productPhotoLogic;
        private readonly KeyToProductLogic keyToProductLogic;
        private readonly favoriteProductLogic favoriteProductLogic;
        private readonly UserManager<ApplicationUser> userManager;
        public ProductController(UserManager<ApplicationUser> userManager, favoriteProductLogic favoriteProductLogic, KeyToProductLogic keyToProductLogic, ProductLogic productLogic, ProductPhotoLogic productPhotoLogic)
        {
            this.productPhotoLogic = productPhotoLogic;
            this.productLogic = productLogic;
            this.userManager = userManager;
            this.favoriteProductLogic = favoriteProductLogic;
            this.keyToProductLogic = keyToProductLogic;
        }
        public IActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ProductShow(int Id)
        {
            var product = await productLogic.ProductDetail(Id);
            var keys = await keyToProductLogic.ReturnSpecialKeyList(Id);
            var cats = product.CategoryToProducts.Select(c => c.Category_Id).ToList();
            var relatedproduct = productLogic.RelatedProduct(cats);
            var model = new ProductShowViewModel()
            {
                product = product,
                specialkeys = keys.Select(k => k.adjKey).ToList(),
                RelatedProduct = relatedproduct,
            };
            return View(model);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LikeProduct(int Id)
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            var favorite = await favoriteProductLogic.FavortieDetail(Id, currentUser.Id);
            var product = await productLogic.ProductDetail(Id);
            if(favorite != null)
            {
                if(await favoriteProductLogic.DeleteFavorite(Id, currentUser.Id))
                {
                    if (product.Like != 0)
                    {
                        product.Like -= 1;
                        if (!await productLogic.UpdateProduct(product))
                        {
                            return Json(new { res = false, Isliked = false });
                        }
                    }
                    return Json(new { res = true, Isliked = false });
                }
                else
                {
                    return Json(new { res = false, Isliked = false });
                }

            }
            else
            {
                var fav = new FavoriteProduct()
                {
                    Product_Id = Id,
                    User_Id = currentUser.Id,
                };
               if(await favoriteProductLogic.AddFavorite(fav))
                {
                    product.Like += 1;
                    if (!await productLogic.UpdateProduct(product))
                    {
                        return Json(new { res = false, Isliked = true });
                    }
                    return Json(new { res = true, Isliked = true });
                }
                else
                {
                    return Json(new { res = true , Isliked = true });
                }
            }
        }
        [Authorize]
        public async Task<IActionResult> IsLike(int Id)
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            var fav = await favoriteProductLogic.FavortieDetail(Id, currentUser.Id);
            if(fav != null)
            {
                return Json(new { islike = true });
            }
            else
            {
                return Json(new { islike = false });
            }
        }
        public IActionResult Search(SearchViewModel model)
        {
            if(model != null)
            {
                var result = productLogic.Search(model.SearchInput,model.CategoryId);
                var viewmodel = new SearchResultViewModel()
                {
                    Products = result,
                    Category_Id = model.CategoryId,
                };
                return View(viewmodel);
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }
        public IActionResult ParameterSearch(string Search="",int Category_Id = 0)
        {
                var result = productLogic.Search(Search,Category_Id);
                var viewmodel = new SearchResultViewModel()
                {
                    Products = result,
                    Category_Id = Category_Id,
                };
                return View("Search",viewmodel);
        }
        public async Task<IActionResult> SearchFilter(SearchFilterViewModel model)
        {
            if(model != null)
            {
                if(model.Values != null)
                {
                    var result = await productLogic.SearchFilter(model.Category_Id,model.IsExist, model.Values, model.FromPrice, model.ToPrice,model.Sortby);
                    var viewModel = new SearchResultViewModel()
                    {
                        Products = result,
                        Category_Id = model.Category_Id,
                    };
                    return View("Search", viewModel);
                }
                else
                {
                    var result = await productLogic.SearchFilter(model.Category_Id,model.IsExist, new List<int>(), model.FromPrice, model.ToPrice,model.Sortby);
                    var viewModel = new SearchResultViewModel()
                    {
                        Products = result,
                        Category_Id = model.Category_Id,
                    };
                    return View("Search", viewModel);
                }
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
