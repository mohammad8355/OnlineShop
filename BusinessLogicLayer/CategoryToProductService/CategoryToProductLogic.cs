using DataAccessLayer.Models;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.CategoryToProductService
{
    public class CategoryToProductLogic
    {
        private readonly MainRepository<CategoryToProduct> CategoryToProductRepository;
        public CategoryToProductLogic(MainRepository<CategoryToProduct> CategoryToProductRepository)
        {
            this.CategoryToProductRepository = CategoryToProductRepository;
        }
        public async Task<bool> AddCategoryToProduct(CategoryToProduct model)
        {
            if (model == null || model.Category_Id == 0 || model.Product_Id == 0)
            {
                return false;
            }
            else
            {
                if (!CategoryToProductRepository.Get(ks => ks.Category_Id == model.Category_Id && ks.Product_Id == model.Product_Id).Result.Any())
                {
                    await CategoryToProductRepository.AddItem(model);
                }
                return true;
            }
        }
        public async Task<bool> DeleteCategoryToProduct(int Category_Id, int Product_Id)
        {
            if (CategoryToProductRepository.Get(s => s.Category_Id == Category_Id && s.Product_Id == Product_Id).Result.Any())
            {
                var categoryToProduct = CategoryToProductRepository.Get(s => s.Category_Id == Category_Id && s.Product_Id == Product_Id).Result.FirstOrDefault();
                await CategoryToProductRepository.DeleteItem(categoryToProduct); return true;
            }
            return false;
        }
        public CategoryToProduct KeytoSubCategoryDetail(int Category_Id, int Product_Id)
        {
            var model = CategoryToProductRepository.Get(s => s.Category_Id == Category_Id && s.Product_Id == Product_Id).Result.FirstOrDefault();
            return model;
        }
        public ICollection<CategoryToProduct> CategoryToProductList()
        {
            return CategoryToProductRepository.Get(null, ks => ks.Category, ks => ks.Product,f => f.Product.ProductPhotos).Result.ToList();
        }
    }
}
