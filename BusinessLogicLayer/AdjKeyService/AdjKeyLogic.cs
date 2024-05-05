using DataAccessLayer.Models;

using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.ReturnMultipleData;

namespace BusinessLogicLayer.AdjKeyService
{
    public class AdjKeyLogic
    {
        private readonly MainRepository<AdjKey> AdjKeyRepository;
        private readonly MainRepository<KeyToSubCategory> KeyToSubCategoryRepository;
        private readonly MainRepository<KeyToProduct> KeyToProductRepository;
        private readonly MainRepository<AdjValue> AdjValueRepository;
        public AdjKeyLogic(MainRepository<AdjKey> AdjKeyRepository, MainRepository<KeyToSubCategory> KeyToSubCategoryRepository, MainRepository<KeyToProduct> KeyToProductRepository, MainRepository<AdjValue> adjValueRepository)
        {
            this.AdjKeyRepository = AdjKeyRepository;
            this.KeyToSubCategoryRepository = KeyToSubCategoryRepository;
            this.KeyToProductRepository = KeyToProductRepository;
            AdjValueRepository = adjValueRepository;
        }
        public async Task<bool> AddAdjKey(AdjKey model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description))
            {
                return false;
            }
            else
            {
                await AdjKeyRepository.AddItem(model);
                return true;
            }
        }
        public async Task<bool> EditAdjKey(AdjKey model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description))
            {
                return false;
            }
            else
            {
                AdjKeyRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteAdjKey(int Id)
        {
            if (KeyToSubCategoryRepository.Get(k => k.key_Id == Id).Result.Any())
            {
                foreach (var item in KeyToSubCategoryRepository.Get(k => k.key_Id == Id).Result.ToList())
                {
                    await KeyToSubCategoryRepository.DeleteItem(item);
                }
            }
            if (KeyToProductRepository.Get(k => k.Key_Id == Id).Result.Any())
            {
                foreach (var item in KeyToProductRepository.Get(k => k.Key_Id == Id).Result.ToList())
                {
                    await KeyToProductRepository.DeleteItem(item);
                }
            }
            if (AdjValueRepository.Get(v => v.adjkey_Id == Id).Result.Any())
            {
                foreach (var item in AdjValueRepository.Get(v => v.adjkey_Id == Id).Result.ToList())
                {
                    await AdjValueRepository.DeleteItem(item.Id);
                }
            }
            if (await AdjKeyRepository.DeleteItem(Id))
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
            var keyToProduct = KeyToProductRepository.Get(kp => kp.Key_Id == Id).Result.ToList();
            var keyToSubcategory = KeyToSubCategoryRepository.Get(ks => ks.key_Id == Id).Result.ToList();
            var AdjValues = AdjValueRepository.Get(v => v.adjkey_Id == Id).Result.ToList();
            model.KeyToProducts = keyToProduct;
            model.keyToSubCategories = keyToSubcategory;
            model.adjValues = AdjValues;
            return model;
        }
        public List<AdjKey> AdjKeyList()
        {

            List<AdjKey> keys = new List<AdjKey>();
            foreach (var item in AdjKeyRepository.Get().Result.ToList())
            {
                var keyToProduct = KeyToProductRepository.Get(kp => kp.Key_Id == item.Id, v => v.product).Result.ToList();
                var keyToSubcategory = KeyToSubCategoryRepository.Get(ks => ks.key_Id == item.Id, k => k.subCategory).Result.ToList();
                var AdjValues = AdjValueRepository.Get(v => v.adjkey_Id == item.Id).Result.ToList();
                item.keyToSubCategories = keyToSubcategory;
                item.KeyToProducts = keyToProduct;
                item.adjValues = AdjValues;
                keys.Add(item);
            }
            return keys;
        }
        public List<AdjKey> AdjKeyListByCategory(int Category_Id)
        {
            if (Category_Id != 0)
            {
                var keys = AdjKeyList().Where(a => a.keyToSubCategories.Select(s => s.SubCategory_Id).Contains(Category_Id)).ToList();
                return keys;
            }
            return new List<AdjKey>();
        }
    }

}