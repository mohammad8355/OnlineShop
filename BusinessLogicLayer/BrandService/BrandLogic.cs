using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
namespace BusinessLogicLayer.BrandService
{
    public class BrandLogic
    {
        private readonly MainRepository<Brand> BrandRepository;
        public BrandLogic(MainRepository<Brand> BrandRepository)
        {
            this.BrandRepository = BrandRepository;
        }
        public async Task<bool> AddBrand(Brand model)
        {
            if(model  == null || string.IsNullOrEmpty(model.Name)) 
            {
                return false;
            }
            else
            {
               await BrandRepository.AddItem(model);
                return true;
            }
        }
        public bool EditBrand(Brand model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                return false;
            }
            else
            {
                BrandRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteBrand(int Id)
        {
            if(Id != 0 && Id != null)
            {
                if(await BrandRepository.DeleteItem(Id))
                {
                    return true;
                }
                
            }
            return false;
        }
        public async Task<Brand> BrandDetail(int Id)
        {
            if(Id != 0 || Id != null)
            {
                var brand = await BrandRepository.Get(b => b.Id == Id);
                return brand.FirstOrDefault();
            }
            return new Brand();
        }
        public async Task<List<Brand>> BrandList()
        {
            var brandList = await BrandRepository.Get();
            return brandList.ToList();
        }
    }
}
