using BusinessEntity;

using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.CategoryService
{
    public class HeadCategoryLogic
    {
        private readonly MainRepository<Category> CategoryRepository;
        private readonly MainRepository<HeadCategory> HeadCategoryRepository;
        private readonly MainRepository<SubCategory> SubCategoryRepository;
        public HeadCategoryLogic(MainRepository<HeadCategory> HeadCategoryRepository, MainRepository<SubCategory> SubCategoryRepository, MainRepository<Category> categoryRepository)
        {
            this.HeadCategoryRepository = HeadCategoryRepository;
            this.SubCategoryRepository = SubCategoryRepository;
            this.CategoryRepository = categoryRepository;

        }
        public async Task<bool> AddHeadCategory(HeadCategory model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.IdentifierName) || string.IsNullOrEmpty(model.Description))
            {
                return false;
            }
            else
            {
                await HeadCategoryRepository.AddItem(model);
                return true;
            }
        }
        public bool UpdateHeadCategory(HeadCategory model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.IdentifierName) || string.IsNullOrEmpty(model.Description))
            {
                return false;
            }
            else
            {
                HeadCategoryRepository.EditItem(model);

                return true;
            }
        }
        public async Task<bool> DeleteHeadCategory(int Id)
        {

            foreach (var category in CategoryRepository.Get(c => c.headCategory_Id == Id).Result.ToList())
            {
                if (SubCategoryRepository.Get(sc => sc.Parent == category.IdentifierName).Result.Any())
                {
                    foreach (var subcategory in SubCategoryRepository.Get(sc => sc.Parent == category.IdentifierName).Result.ToList())
                    {
                        if (!await SubCategoryRepository.DeleteItem(subcategory.Id))
                            return false;
                    }
                }
                if (!await CategoryRepository.DeleteItem(category.Id))
                    return false;
            }
            if (await HeadCategoryRepository.DeleteItem(Id))
            {
                return true;
            }
            return false;
        }
        public ICollection<HeadCategory> HeadCategoryList()
        {
            ICollection<HeadCategory> categories = new List<HeadCategory>();
            foreach (var item in HeadCategoryRepository.Get().Result.ToList())
            {
                if (CategoryRepository.Get(s => s.headCategory_Id == item.Id).Result.Any())
                {
                    var categoryList = new List<Category>();
                    foreach (var subItem in CategoryRepository.Get(s => s.headCategory_Id == item.Id).Result.ToList())
                    {
                        var subcategoryList = new List<SubCategory>();
                        if (SubCategoryRepository.Get(sc => sc.category_Id == subItem.Id).Result.Any())
                        {
                            foreach (var subcategory in SubCategoryRepository.Get(sc => sc.category_Id == subItem.Id).Result.ToList())
                            {
                                subcategoryList.Add(subcategory);
                            }
                            subItem.SubCategories = subcategoryList;
                        }
                        categoryList.Add(subItem);
                    }
                    item.Categories = categoryList;
                }
                categories.Add(item);
            }
            return categories;
        }
        public HeadCategory HeadCategoryDetail(int Id)
        {
            var Headcategory = HeadCategoryRepository.Get(hc => hc.Id == Id).Result.FirstOrDefault();
            var category = CategoryRepository.Get(c => c.headCategory_Id == Headcategory.Id).Result.ToList();

            foreach (var c in category)
            {
                c.SubCategories = SubCategoryRepository.Get(sc => sc.category_Id == c.Id).Result.ToList();

            };
            Headcategory.Categories = category;
            return Headcategory;
        }
    }
}