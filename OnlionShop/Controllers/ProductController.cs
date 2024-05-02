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
                return View();
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

    }
}
