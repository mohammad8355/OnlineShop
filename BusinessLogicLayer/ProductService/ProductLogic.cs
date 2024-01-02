﻿ 
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessEntity;

namespace BusinessLogicLayer.ProductService
{
    public class ProductLogic
    {
        private readonly MainRepository<Product> ProductRepository;
        private readonly MainRepository<SubCategory> SubCategoryRepository;
        private readonly MainRepository<KeyToProduct> KeyToProductRepository;
        private readonly MainRepository<DiscountToProduct> DiscountToProductRepository;
        private readonly MainRepository<ProductPhoto> productphotoRepository;
        public ProductLogic(MainRepository<Product> ProductRepository, MainRepository<SubCategory> SubRepository, MainRepository<KeyToProduct> KeyToProductRepository, MainRepository<DiscountToProduct> discountToProductRepository, MainRepository<ProductPhoto> productphotoRepository)
        {
            this.ProductRepository = ProductRepository;
            SubCategoryRepository = SubRepository;
            this.KeyToProductRepository = KeyToProductRepository;
            DiscountToProductRepository = discountToProductRepository;
            this.productphotoRepository = productphotoRepository;

        }

        public async Task<bool> AddProduct(Product model)
        {
            if(model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description) || model.SubCategory_Id == 0)
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
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description) || model.SubCategory_Id == 0)
            {
                return false;
            }
            else
            {
                await ProductRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteProduct(int Id)
        {
            if(await ProductRepository.DeleteItem(Id))
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
                model.subCategory = SubCategoryRepository.Get(s => s.Id == model.SubCategory_Id).Result.FirstOrDefault();
                model.keyToProducts = KeyToProductRepository.Get(k => k.Product_Id == model.Id).Result.ToList();
                model.discountToProducts = DiscountToProductRepository.Get(d => d.Product_Id == model.Id).Result.ToList();
                model.ProductPhotos = productphotoRepository.Get(p => p.Product_Id == model.Id).Result.ToList();
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
                item.subCategory = SubCategoryRepository.Get(s => s.Id == item.SubCategory_Id).Result.FirstOrDefault();
                item.keyToProducts = KeyToProductRepository.Get(k => k.Product_Id == item.Id).Result.ToList();
                item.discountToProducts = DiscountToProductRepository.Get(d => d.Product_Id == item.Id).Result.ToList();
                item.ProductPhotos = productphotoRepository.Get(p => p.Product_Id == item.Id).Result.ToList();
                products.Add(item);
            }
            return products;
        }

    }
}
