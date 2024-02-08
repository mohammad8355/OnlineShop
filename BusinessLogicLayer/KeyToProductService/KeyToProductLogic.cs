using DataAccessLayer.Models;
using DataAccessLayer.services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (!KeyToProductRepository.Get(ks => ks.Key_Id == model.Key_Id && ks.Product_Id == model.Product_Id).Result.Any())
                {
                    await KeyToProductRepository.AddItem(model);
                }
                return true;
            }
        }
        //public async Task<bool> DeleteKeyToProductFromKey(int Id)
        //{
        //    if (KeyToProductRepository.Get(s => s.key_Id == Id).Result.Any())
        //    {
        //        await KeyToProductRepository.DeleteItem(Id); return true;
        //    }
        //    return false;
        //}
        public async Task<bool> DeleteKeyToProduct(int key_Id, int Product_Id)
        {
            if (KeyToProductRepository.Get(s => s.Key_Id == key_Id && s.Product_Id == Product_Id).Result.Any())
            {
                var KeyToProduct = KeyToProductRepository.Get(s => s.Product_Id == Product_Id && s.Key_Id == key_Id).Result.FirstOrDefault();
                await KeyToProductRepository.DeleteItem(KeyToProduct); return true;
            }
            return false;
        }
        public KeyToProduct KeyToProductDetail(int key_Id, int Product_Id)
        {
            var model = KeyToProductRepository.Get(ks => ks.Key_Id == key_Id && ks.Product_Id == Product_Id).Result.FirstOrDefault();
            return model;
        }
        public ICollection<KeyToProduct> KeyToProductList()
        {


            return KeyToProductRepository.Get(null, ks => ks.product, ks => ks.adjKey).Result.ToList();
        }
    }

}