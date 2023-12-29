using BusinessEntity;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.CategoryService
{
    public class SubCategoryLogic
    {
        private readonly MainRepository<SubCategory> SubCategoryRepository;
        public SubCategoryLogic( MainRepository<SubCategory> SubCategoryRepository)
        {
            this.SubCategoryRepository = SubCategoryRepository;
        }
        public async Task<bool> AddSubCategory(SubCategory model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.Parent) || model.category_Id == 0)
            {
                return false;
            }
            else
            {
                await SubCategoryRepository.AddItem(model);
                return true;
            }
        }
        public async Task<bool> UpdateSubCategory(SubCategory model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.Parent) || model.category_Id == 0)
            {
                return false;
            }
            else
            {
                await SubCategoryRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteSubCategory(int Id)
        {
            if (await SubCategoryRepository.DeleteItem(Id))
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        public ICollection<SubCategory> SubCategoryList()
        {
            ICollection<SubCategory> categories = new List<SubCategory>();
            foreach (var item in SubCategoryRepository.Get().Result.ToList())
            {
                categories.Add(item);
            }
            return categories;
        }
        public SubCategory SubCategoryDetail(int Id)
        {
            return SubCategoryRepository.Get(c => c.Id == Id).Result.FirstOrDefault();
        }
    }
}
