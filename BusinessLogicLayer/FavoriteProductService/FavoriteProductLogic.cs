using DataAccessLayer.Models;
using DataAccessLayer.services;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.favoriteProductService
{
    public class favoriteProductLogic
    {
        private readonly MainRepository<FavoriteProduct> favoriteProductRepository;
        public favoriteProductLogic(MainRepository<FavoriteProduct> favoriteProductRepository)
        {
            this.favoriteProductRepository = favoriteProductRepository;
        }
        public async Task<bool> AddFavorite(FavoriteProduct model)
        {
            if(model == null || model.Product_Id == 0 || string.IsNullOrEmpty(model.User_Id))
            {
                return false;
            }
            else
            {
                await favoriteProductRepository.AddItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteFavorite(int product_Id,string User_Id)
        {
            if(product_Id != 0)
            {
                var favoriteProduct = await favoriteProductRepository.Get(fp => fp.Product_Id == product_Id && fp.User_Id == User_Id).ToListAsync();
                if (favoriteProduct.First() != null)
                {
                    await favoriteProductRepository.DeleteItem(favoriteProduct.First());
                    return true;
                }
            }
            return false;
        }
        public async Task<FavoriteProduct> FavortieDetail(int product_Id, string User_Id)
        {
            var fav = await favoriteProductRepository.Get(f => f.Product_Id == product_Id && f.User_Id == User_Id).ToListAsync();
            return fav.FirstOrDefault();
        }
        public async Task<List<FavoriteProduct>> LikedProducts(string User_Id)
        {
            var favs = await
                favoriteProductRepository
                    .Get(f => 
                        f.User_Id == User_Id, f => f.Product, K => K.Product.ProductPhotos).ToListAsync();
            return favs.ToList();
        }
        public async Task<List<FavoriteProduct>> UsersLikedProduct(int Product_Id)
        {
            var favs = await favoriteProductRepository
                .Get(f => f.Product_Id == Product_Id, f => f.User,f => f.User.Commnets).ToListAsync();
            return favs.ToList();
        }
    }
}
