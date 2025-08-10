using DataAccessLayer.Models;
using DataAccessLayer.services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.KeyToProductService
{
    public class KeyToProductLogic
    {

        private readonly MainRepository<KeyToProduct> KeyToProductRepository;
        public KeyToProductLogic(MainRepository<KeyToProduct> KeyToProductRepository)
        {
            this.KeyToProductRepository = KeyToProductRepository;
        }
        public async Task<bool> AddKeyToProduct(KeyToProduct model)
        {
            if (model == null || model.Product_Id == 0 || model.Key_Id == 0)
            {
                return false;
            }
            else
            {
                if (!KeyToProductRepository.Get(ks => ks.Key_Id == model.Key_Id && ks.Product_Id == model.Product_Id).Any())
                {
                    await KeyToProductRepository.AddItem(model);
                }
                return true;
            }
        }
        public async Task<bool> DeleteKeyToProduct(int key_Id, int Product_Id)
        {
            if (KeyToProductRepository.Get(s => s.Key_Id == key_Id && s.Product_Id == Product_Id).Any())
            {
                var KeyToProduct = KeyToProductRepository.Get(s => s.Product_Id == Product_Id && s.Key_Id == key_Id).FirstOrDefault();
                await KeyToProductRepository.DeleteItem(KeyToProduct); return true;
            }
            return false;
        }
        public KeyToProduct KeyToProductDetail(int key_Id, int Product_Id)
        {
            var model = KeyToProductRepository.Get(ks => ks.Key_Id == key_Id && ks.Product_Id == Product_Id).FirstOrDefault();
            return model;
        }
        public ICollection<KeyToProduct> KeyToProductList()
        {


            return KeyToProductRepository.Get(null, ks => ks.product, ks => ks.adjKey).ToList();
        }
        public async Task<KeyToProduct> ReturnSpecialKey(int key_Id,int Product_Id)
        {
            var keyToProduct = await KeyToProductRepository
                .Get(kp => kp.Key_Id == key_Id && kp.Product_Id == 
                    Product_Id && kp.IsSpecial == true, ks => ks.adjKey).ToListAsync();
            return keyToProduct.FirstOrDefault();
        }
        public async Task<List<KeyToProduct>> ReturnSpecialKeyList(int Product_Id)
        {
            var keyToProduct = await KeyToProductRepository.Get(kp => kp.Product_Id ==
                Product_Id && kp.IsSpecial == true, ks => ks.adjKey).ToListAsync();
            return keyToProduct.ToList();
        }
        public bool EditKeyToProduct(KeyToProduct model)
        {
            if(model != null && model.Key_Id != 0 && model.Product_Id != 0)
            {
                KeyToProductRepository.EditItem(model);
                return true;
            }
            return false;
        }
    }

}