using BusinessEntity;
 
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
        private readonly MainRepository<SubCategory> SubCategoryRepository;
        public CategoryLogic(MainRepository<Category> CategoryRepository,MainRepository<SubCategory> SubCategoryRepository)
        {
            this.CategoryRepository = CategoryRepository;
            this.SubCategoryRepository = SubCategoryRepository;
        }
        public async Task<bool> AddCategory(Category model)
        {
            if(string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.IdentifierName) || string.IsNullOrEmpty(model.Description))
            {
                return false;
            }
            else
            {
             await CategoryRepository.AddItem(model);
                return true;
            }
        }
        public async Task<bool> UpdateCategory(Category model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.IdentifierName) || string.IsNullOrEmpty(model.Description))
            {
                return false;
            }
            else
            {
                await CategoryRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteCategory(int Id)
        {
            if (SubCategoryRepository.Get(c => c.category_Id == Id).Result.Count() > 0)
            {
                foreach(var item in SubCategoryRepository.Get(c => c.category_Id == Id).Result.ToList())
                {
                   await SubCategoryRepository.DeleteItem(item.Id);
                }
              if(await CategoryRepository.DeleteItem(Id))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
              if(await CategoryRepository.DeleteItem(Id))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public ICollection<Category> CategoryList()
        {
            ICollection<Category> categories = new List<Category>();
            foreach (var item in CategoryRepository.Get().Result.ToList())
            {
               if(SubCategoryRepository.Get(s => s.category_Id == item.Id).Result.Any())
                {
                    var subcategoryList = new List<SubCategory>();
                    foreach(var subItem in SubCategoryRepository.Get(s => s.category_Id == item.Id).Result.ToList())
                    {
                        subcategoryList.Add(subItem);
                    }
                    item.subCategories = subcategoryList;
                }
               categories.Add(item);
            }
            return categories;
        }
        public Category CategoryDetail(int Id)
        {
            return CategoryRepository.Get(c => c.Id == Id).Result.FirstOrDefault();
        }
    }
}
