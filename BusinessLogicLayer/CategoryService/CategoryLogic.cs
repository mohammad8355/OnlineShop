using DataAccessLayer.Models;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.CategoryService
{
    public class CategoryLogic
    {
        private readonly MainRepository<Category> CategoryRepository;
        private readonly MainRepository<AdjKey> adjkeyRepository;
        public CategoryLogic( MainRepository<Category> CategoryRepository, MainRepository<AdjKey> adjkeyRepository)
        {
            this.CategoryRepository = CategoryRepository;
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
            foreach (var item in CategoryRepository.Get(null,v => v.keyToSubCategories ,f => f.ChildCategories,i => i.ParentCategory ).Result.ToList())
            {
                foreach(var key in item.keyToSubCategories)
                {
                    key.adjKey = adjkeyRepository.Get(k => k.Id == key.key_Id).Result.FirstOrDefault();
                }
                categories.Add(item);
            }
            return categories;
        }
        public Category CategoryDetail(int Id)
        {
            return CategoryRepository.Get(c => c.Id == Id,v => v.keyToSubCategories ,o => o.ChildCategories).Result.FirstOrDefault();
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
