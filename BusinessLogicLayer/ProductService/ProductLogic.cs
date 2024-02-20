using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Models;
using BusinessLogicLayer.CategoryToProductService;
using BusinessLogicLayer.ProductPhotoService;
using BusinessLogicLayer.AdjValueService;

namespace BusinessLogicLayer.ProductService
{
    public class ProductLogic
    {
        private readonly MainRepository<Product> ProductRepository;
        private readonly MainRepository<Category> SubCategoryRepository;
        private readonly MainRepository<KeyToProduct> KeyToProductRepository;
        private readonly MainRepository<DiscountToProduct> DiscountToProductRepository;
        private readonly MainRepository<ProductPhoto> productphotoRepository;
        private readonly CategoryToProductLogic categoryToProductLogic;
        private readonly MainRepository<AdjValue> adjValueLogic;
        public ProductLogic(MainRepository<AdjValue> adjValueLogic, CategoryToProductLogic categoryToProductLogic,MainRepository<Product> ProductRepository, MainRepository<Category> SubRepository, MainRepository<KeyToProduct> KeyToProductRepository, MainRepository<DiscountToProduct> discountToProductRepository, MainRepository<ProductPhoto> productphotoRepository)
        {
            this.ProductRepository = ProductRepository;
            SubCategoryRepository = SubRepository;
            this.KeyToProductRepository = KeyToProductRepository;
            DiscountToProductRepository = discountToProductRepository;
            this.productphotoRepository = productphotoRepository;
            this.categoryToProductLogic = categoryToProductLogic;
            this.adjValueLogic = adjValueLogic;

        }

        public async Task<bool> AddProduct(Product model)
        {
            if(model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.ProductCode) )
            {
                return false;
            }
            else
            {
               await ProductRepository.AddItem(model);
                return true;
            }
        }
        public async Task<bool> UpdateProduct(Product model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.ProductCode))
            {
                return false;
            }
            else
            {
                 ProductRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteProduct(int Id)
        {
            if(categoryToProductLogic.CategoryToProductList().Where(cp => cp.Product_Id == Id).Any())
            foreach (var catpro in categoryToProductLogic.CategoryToProductList().Where(cp => cp.Product_Id == Id).ToList())
            {
                await categoryToProductLogic.DeleteCategoryToProduct(catpro.Category_Id, catpro.Product_Id);
            };

            if (productphotoRepository.Get(pp => pp.Product_Id == Id).Result.Any())
                foreach (var photo in productphotoRepository.Get(pp => pp.Product_Id == Id).Result.ToList())
            {
                await productphotoRepository.DeleteItem(photo.Id);
            };
            if (await ProductRepository.DeleteItem(Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<Product> ProductDetail(int Id)
        {
            if(ProductRepository.Get(p => p.Id == Id).Result.Any())
            {
                var model = ProductRepository.Get(p => p.Id == Id).Result.FirstOrDefault();
                model.keyToProducts = KeyToProductRepository.Get(k => k.Product_Id == model.Id,v => v.adjKey).Result.ToList();
                foreach (var key in model.keyToProducts)
                {
                    var values = adjValueLogic.Get(v => v.adjkey_Id == key.Key_Id).Result.ToList();
                    key.adjKey.adjValues = values;
                }
                model.discountToProducts = DiscountToProductRepository.Get(d => d.Product_Id == model.Id).Result.ToList();
                model.ProductPhotos = productphotoRepository.Get(p => p.Product_Id == model.Id).Result.ToList();
                model.CategoryToProducts = categoryToProductLogic.CategoryToProductList().Where(cp => cp.Product_Id == model.Id).ToList();
                return model;
            }
            else
            {
                return new Product();
            }
        }
        public ICollection<Product> ProductList()
        {
            ICollection<Product> products = new List<Product>();
            foreach(var item in ProductRepository.Get().Result.ToList())
            {
                item.keyToProducts = KeyToProductRepository.Get(k => k.Product_Id == item.Id,k => k.adjKey).Result.ToList();
                foreach(var key in item.keyToProducts)
                {
                    var values = adjValueLogic.Get(v => v.adjkey_Id == key.Key_Id).Result.ToList();
                    key.adjKey.adjValues = values;
                }
                item.discountToProducts = DiscountToProductRepository.Get(d => d.Product_Id == item.Id,v => v.discount).Result.ToList();
                item.ProductPhotos = productphotoRepository.Get(p => p.Product_Id == item.Id).Result.ToList();
                item.CategoryToProducts = categoryToProductLogic.CategoryToProductList().Where(cp => cp.Product_Id == item.Id).ToList();
                products.Add(item);
            }
            return products;
        }

    }
}
