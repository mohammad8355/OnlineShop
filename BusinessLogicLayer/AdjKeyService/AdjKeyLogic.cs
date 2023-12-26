using BusinessEntity;
using BusinessEntity.Models;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.AdjKeyService
{
    public class AdjKeyLogic
    {
        private readonly MainRepository<AdjKey> AdjKeyRepository;
        private readonly MainRepository<KeyToSubCategory> KeyToSubCategoryRepository;
        private readonly MainRepository<KeyToProduct> KeyToProductRepository;
        public AdjKeyLogic(MainRepository<AdjKey> AdjKeyRepository, MainRepository<KeyToSubCategory> KeyToSubCategoryRepository, MainRepository<KeyToProduct> KeyToProductRepository)
        {
            this.AdjKeyRepository = AdjKeyRepository;
            this.KeyToSubCategoryRepository = KeyToSubCategoryRepository;
            this.KeyToProductRepository = KeyToProductRepository;
        }
        public async Task<bool> AddAdjKey(AdjKey model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.DataType) || model.keyToSubCategories != null || model.KeyToProducts != null)
            {
                return false;
            }
            else
            {
                 await AdjKeyRepository.AddItem(model);
                foreach (var item in model.keyToSubCategories)
                {
                  await  KeyToSubCategoryRepository.AddItem(item);

                }
                foreach (var item in model.KeyToProducts)
                {
                  await  KeyToProductRepository.AddItem(item);

                }
                return true;
            }
        }
        public async Task<bool> EditAdjKey(AdjKey model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.DataType) || model.keyToSubCategories != null || model.KeyToProducts != null)
            {
                return false;
            }
            else
            {
                await AdjKeyRepository.EditItem(model);
                foreach (var item in model.keyToSubCategories)
                {
                    await KeyToSubCategoryRepository.EditItem(item);

                }
                foreach (var item in model.KeyToProducts)
                {
                    await KeyToProductRepository.EditItem(item);

                }
                return true;
            }
        }
        public async Task<bool> DeleteAdjKey(int Id)
        {
            if(KeyToSubCategoryRepository.Get(k => k.key_Id == Id).Result.Any())
            {
                foreach(var item in KeyToSubCategoryRepository.Get(k => k.key_Id == Id).Result.ToList())
                {
                    if (await KeyToSubCategoryRepository.DeleteItem(item.key_Id))
                        return true;
                }
            }
            if (KeyToProductRepository.Get(k => k.Key_Id == Id).Result.Any())
            {
                foreach (var item in KeyToProductRepository.Get(k => k.Key_Id == Id).Result.ToList())
                {
                    if (await KeyToSubCategoryRepository.DeleteItem(item.Key_Id))
                        return true;
                }
            }
            if (await  AdjKeyRepository.DeleteItem(Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public AdjKey AdjKeyDetail(int Id)
        {
            var model = AdjKeyRepository.Get(k => k.Id == Id).Result.FirstOrDefault();
            if(model != null)
            {
                var keyToProduct = KeyToProductRepository.Get(kp => kp.Key_Id == Id).Result.ToList();
                var keyToSubcategory = KeyToSubCategoryRepository.Get(ks => ks.key_Id == Id).Result.ToList();
                model.KeyToProducts = keyToProduct;
                model.keyToSubCategories = keyToSubcategory;
                return model;
            }
            else
            {
                return new AdjKey();
            }
        }
        public ICollection<AdjKey> AdjKeyList()
        {

            ICollection<AdjKey> keys = new List<AdjKey>();
            foreach(var item in AdjKeyRepository.Get().Result.ToList())
            {
                var keyToProduct = KeyToProductRepository.Get(kp => kp.Key_Id == item.Id).Result.ToList();
                var keyToSubcategory = KeyToSubCategoryRepository.Get(ks => ks.key_Id == item.Id).Result.ToList();
                item.keyToSubCategories = keyToSubcategory;
                item.KeyToProducts = keyToProduct;
                keys.Add(item);
            }
            return keys;
        }
    }

}