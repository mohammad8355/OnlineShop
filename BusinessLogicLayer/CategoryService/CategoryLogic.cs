using BusinessLogicLayer.CategoryToProductService;
using BusinessLogicLayer.KeyToSubCategoryService;
using DataAccessLayer.Models;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.CategoryService.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.CategoryService
{
    public class CategoryLogic
    {
        private readonly MainRepository<Category> CategoryRepository;
        private readonly MainRepository<AdjKey> adjkeyRepository;
        private readonly CategoryToProductLogic _categoryToProductLogic;
        private readonly KeyToSubCategoryLogic _keyToSubCategoryLogic;
        public CategoryLogic(KeyToSubCategoryLogic keyToSubCategoryLogic,CategoryToProductLogic categoryToProductLogic, MainRepository<Category> CategoryRepository, MainRepository<AdjKey> adjkeyRepository)
        {
            this.CategoryRepository = CategoryRepository;
            _categoryToProductLogic = categoryToProductLogic;
            _keyToSubCategoryLogic = keyToSubCategoryLogic;
            this.adjkeyRepository = adjkeyRepository;
        }
        public async Task<bool> AddCategory(Category model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description))
            {
                return false;
            }
            else
            {
                await CategoryRepository.AddItem(model);
                return true;
            }
        }
        public  bool UpdateCategory(Category model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description))
            {
                return false;
            }
            else
            {
                 CategoryRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> AttachCategory(Category model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description))
            {
                return false;
            }
            else
            {
               await CategoryRepository.Attach(model);
                return true;
            }
        }
        public async Task<bool> DeleteCategory(int Id)
        {
            if(CategoryList().Where(c => c.ParentId == Id).Any())
            {
                foreach(var childcat in CategoryList().Where(c => c.ParentId == Id).ToList())
                {
                    await CategoryRepository.DeleteItem(childcat.Id);
                }
            }
            if(_categoryToProductLogic.CategoryToProductList().Where(c => c.Category_Id == Id).Any())
            {
                foreach(var cp in _categoryToProductLogic.CategoryToProductList().Where(c => c.Category_Id == Id).ToList())
                {
                    await _categoryToProductLogic.DeleteCategoryToProduct(cp.Category_Id, cp.Product_Id);
                }
            }
            if(_keyToSubCategoryLogic.KeyToSubCategoryList().Where(ks => ks.SubCategory_Id == Id).Any())
            {
                foreach(var ks in _keyToSubCategoryLogic.KeyToSubCategoryList().Where(ks => ks.SubCategory_Id == Id).ToList()){
                    await _keyToSubCategoryLogic.DeleteKeyToSubCategory(ks.key_Id,ks.SubCategory_Id);
                }
            }
            if (await CategoryRepository.DeleteItem(Id))
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        public ICollection<Category> CategoryList()
        {
            ICollection<Category> categories = new List<Category>();
            foreach (var item in 
                     CategoryRepository.Get(null,v => v.keyToSubCategories
                             ,f => f.ChildCategories,i => i.ParentCategory ).
                         ToList())
            {
                foreach(var key in item.keyToSubCategories)
                {
                    key.adjKey = adjkeyRepository.Get(k => k.Id == key.key_Id).FirstOrDefault();
                }
                categories.Add(item);
            }
            return categories;
        }

        public async Task<List<CategoryNameAndIdListDto>> GetCategoryNameAndIdList()
        {
            var list = await CategoryRepository.Get(c => c.ParentId == null).Select(c => new CategoryNameAndIdListDto()
            {
                Name = c.Name,
                Id = c.Id,
            }).ToListAsync();;
            return list;
        }
        public Category CategoryDetail(int Id)
        {
            return CategoryRepository.Get(c => c.Id == Id,v => v.keyToSubCategories ,o => o.ChildCategories).FirstOrDefault();
        }
        public List<Category> CategoryParent()
        {
            return CategoryList().Where(c => c.ParentId == null
            && c.ParentCategory == null).ToList();
        }
        public List<Category> CategoryChildren(int parent_Id)
        {
            return CategoryList().Where(c => c.ParentId == parent_Id).ToList();
        }
    }
}
