using BusinessEntity;
 
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DiscountService
{
    public class DiscountLogic
    {
        private readonly MainRepository<Discount> DiscountRepository;
        private readonly MainRepository<DiscountToProduct> DiscountToProductRepository;
        public DiscountLogic(MainRepository<Discount> DiscountRepository, MainRepository<DiscountToProduct> DiscountToProductRepository)
        {
            this.DiscountRepository = DiscountRepository;
            this.DiscountToProductRepository = DiscountToProductRepository;
        }
        public async Task<bool> AddDiscount(Discount model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.DateBase) || model.Value != 0 || string.IsNullOrEmpty(model.DiscountCode))
            {
                await DiscountRepository.AddItem(model);
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> EditDiscount(Discount model)
        {

            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.DateBase) || model.Value != 0)
            {
                 DiscountRepository.EditItem(model);
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> DeleteDiscount(int Id)
        {
            if (await DiscountRepository.DeleteItem(Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<Discount> DiscountDetails(int Id)
        {
            if (DiscountRepository.Get(p => p.Id == Id).Result.Any())
            {
                var discount = DiscountRepository.Get(d => d.Id == Id).Result.First();
                discount.discountToProducts = DiscountToProductRepository.Get(d => d.Discount_Id == Id).Result.ToList();
                return discount;
            }
            else
            {
                return new Discount();
            }
        }
       public async Task<ICollection<Discount>> DiscountList()
        {
            ICollection<Discount> discountList = new List<Discount>();
            foreach(var item in DiscountRepository.Get().Result.ToList())
            {
                var discountToProduct = DiscountToProductRepository.Get(d => d.Discount_Id == item.Id).Result.ToList();
                item.discountToProducts = discountToProduct;
                discountList.Add(item);
            }
            return discountList;

        }

    }
}
