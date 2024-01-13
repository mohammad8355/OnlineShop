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
        public CategoryLogic(MainRepository<SubCategory> SubCategoryRepository, MainRepository<Category> categoryRepository)
        {
            this.SubCategoryRepository = SubCategoryRepository;
            CategoryRepository = categoryRepository;

        }
        public async Task<bool> AddCategory(Category model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.IdentifierName) || string.IsNullOrEmpty(model.Description) || model.headCategory_Id == 0)
            {
                return false;
            }
            else
            {
                await CategoryRepository.AddItem(model);
                return true;
            }
        }
        public bool UpdateCategory(Category model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.IdentifierName) || string.IsNullOrEmpty(model.Description) || model.headCategory_Id == 0)
            {
                return false;
            }
            else
            {
                CategoryRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteCategory(int Id)
        {
            if (SubCategoryRepository.Get(sc => sc.category_Id == Id).Result.Any())
            {
                foreach (var subcategory in SubCategoryRepository.Get(sc => sc.Id == Id).Result.ToList())
                {
                    if (!await SubCategoryRepository.DeleteItem(subcategory.Id))
                        return false;
                }
            }
            if (await CategoryRepository.DeleteItem(Id))
            {
                return true;
            }
            return false;
        }
        public ICollection<Category> CategoryList()
        {
            ICollection<Category> categories = new List<Category>();
            foreach (var item in CategoryRepository.Get().Result.ToList())
            {
                if (SubCategoryRepository.Get(s => s.category_Id == item.Id).Result.Any())
                {
                    var subcategoryList = new List<SubCategory>();
                    foreach (var subItem in SubCategoryRepository.Get(s => s.category_Id == item.Id).Result.ToList())
                    {
                        subcategoryList.Add(subItem);
                    }
                    item.SubCategories = subcategoryList;
                }
                categories.Add(item);
            }
            return categories;
        }
        public Category CategoryDetail(int Id)
        {
            var Category = CategoryRepository.Get(c => c.Id == Id).Result.FirstOrDefault();
            Category.SubCategories = SubCategoryRepository.Get(sc => sc.category_Id == Category.Id).Result.ToList();
            return Category;
        }
    }
}


