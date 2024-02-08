using DataAccessLayer.Models;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.KeyToSubCategoryService
{
    public class KeyToSubCategoryLogic
    {

        private readonly MainRepository<KeyToSubCategory> KeyToSubCategoryRepository;
        public KeyToSubCategoryLogic(MainRepository<KeyToSubCategory> KeyToSubCategoryRepository)
        {
            this.KeyToSubCategoryRepository = KeyToSubCategoryRepository;
        }
        public async Task<bool> AddKeyToSubCategory(KeyToSubCategory model)
        {
            if (model == null || model.key_Id == 0 || model.SubCategory_Id == 0)
            {
                return false;
            }
            else
            {
                if(!KeyToSubCategoryRepository.Get(ks => ks.key_Id == model.key_Id && ks.SubCategory_Id == model.SubCategory_Id).Result.Any())
                {
                    await KeyToSubCategoryRepository.AddItem(model);
                }
                return true;
            }
        }
        //public async Task<bool> DeleteKeyToSubCategoryFromKey(int Id)
        //{
        //    if (KeyToSubCategoryRepository.Get(s => s.key_Id == Id).Result.Any())
        //    {
        //        await KeyToSubCategoryRepository.DeleteItem(Id); return true;
        //    }
        //    return false;
        //}
        public async Task<bool> DeleteKeyToSubCategory(int key_Id,int subcategory_Id)
        {
            if (KeyToSubCategoryRepository.Get(s => s.SubCategory_Id == subcategory_Id && s.key_Id == key_Id).Result.Any())
            {
                var keytosubcategory = KeyToSubCategoryRepository.Get(s => s.SubCategory_Id == subcategory_Id && s.key_Id == key_Id).Result.FirstOrDefault();
                await KeyToSubCategoryRepository.DeleteItem(keytosubcategory); return true;
            }
            return false;
        }
        public KeyToSubCategory KeytoSubCategoryDetail(int key_Id, int SubCategory_Id)
        {
            var model = KeyToSubCategoryRepository.Get(ks => ks.key_Id == key_Id && ks.SubCategory_Id == SubCategory_Id).Result.FirstOrDefault();
            return model;
        }
        public ICollection<KeyToSubCategory> KeyToSubCategoryList()
        {


            return KeyToSubCategoryRepository.Get(null, ks => ks.subCategory, ks => ks.adjKey).Result.ToList();
        }
    }
}


