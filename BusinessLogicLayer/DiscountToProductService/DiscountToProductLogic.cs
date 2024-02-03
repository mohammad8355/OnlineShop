using BusinessEntity;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DiscountToProductService
{
    public class DiscountToProductLogic
    {

        private readonly MainRepository<DiscountToProduct> DiscountToProductRepository;
        public DiscountToProductLogic(MainRepository<DiscountToProduct> DiscountToProductRepository)
        {
            this.DiscountToProductRepository = DiscountToProductRepository;
        }
        public async Task<bool> AddDiscountToProduct(DiscountToProduct model)
        {
            if (model == null || model.Discount_Id == 0 || model.Product_Id == 0)
            {
                return false;
            }
            else
            {
                if (!DiscountToProductRepository.Get(ks => ks.Discount_Id == model.Discount_Id && ks.Product_Id == model.Product_Id).Result.Any())
                {
                    await DiscountToProductRepository.AddItem(model);
                }
                return true;
            }
        }
        //public async Task<bool> DeleteDiscountToProductFromKey(int Id)
        //{
        //    if (DiscountToProductRepository.Get(s => s.Discount_Id == Id).Result.Any())
        //    {
        //        await DiscountToProductRepository.DeleteItem(Id); return true;
        //    }
        //    return false;
        //}
        public async Task<bool> DeleteDiscountToProduct(int Discount_Id, int Product_Id)
        {
            if (DiscountToProductRepository.Get(s => s.Product_Id == Product_Id && s.Discount_Id == Discount_Id).Result.Any())
            {
                var DiscountToProduct = DiscountToProductRepository.Get(s => s.Product_Id == Product_Id && s.Discount_Id == Discount_Id).Result.FirstOrDefault();
                await DiscountToProductRepository.DeleteItem(DiscountToProduct); return true;
            }
            return false;
        }
        public DiscountToProduct DiscountToProductDetail(int Discount_Id, int Product_Id)
        {
            var model = DiscountToProductRepository.Get(ks => ks.Discount_Id == Discount_Id && ks.Product_Id == Product_Id).Result.FirstOrDefault();
            return model;
        }
        public ICollection<DiscountToProduct> DiscountToProductList()
        {


            return DiscountToProductRepository.Get(null, ks => ks.discount, ks => ks.product).Result.ToList();
        }
    }

}
