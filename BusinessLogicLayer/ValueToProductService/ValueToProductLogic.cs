using DataAccessLayer.Models;
using DataAccessLayer.services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ValueToProductService
{
    public class ValueToProductLogic
    {

        private readonly MainRepository<ValueToProduct> ValueToProductRepository;
        public ValueToProductLogic(MainRepository<ValueToProduct> ValueToProductRepository)
        {
            this.ValueToProductRepository = ValueToProductRepository;
        }
        public async Task<bool> AddValueToProduct(ValueToProduct model)
        {
            if (model == null || model.Product_Id == 0 || model.Value_Id == 0)
            {
                return false;
            }
            else
            {
                if (!ValueToProductRepository.Get(ks => ks.Value_Id == model.Value_Id && ks.Product_Id == model.Product_Id).Result.Any())
                {
                    await ValueToProductRepository.AddItem(model);
                }
                return true;
            }
        }
        //public async Task<bool> DeleteValueToProductFromKey(int Id)
        //{
        //    if (ValueToProductRepository.Get(s => s.Value_Id == Id).Result.Any())
        //    {
        //        await ValueToProductRepository.DeleteItem(Id); return true;
        //    }
        //    return false;
        //}
        public async Task<bool> DeleteValueToProduct(int Value_Id, int Product_Id)
        {
            if (ValueToProductRepository.Get(s => s.Value_Id == Value_Id && s.Product_Id == Product_Id).Result.Any())
            {
                var ValueToProduct = ValueToProductRepository.Get(s => s.Product_Id == Product_Id && s.Value_Id == Value_Id).Result.FirstOrDefault();
                await ValueToProductRepository.DeleteItem(ValueToProduct); return true;
            }
            return false;
        }
        public ValueToProduct ValueToProductDetail(int Value_Id, int Product_Id)
        {
            var model = ValueToProductRepository.Get(ks => ks.Value_Id == Value_Id && ks.Product_Id == Product_Id).Result.FirstOrDefault();
            return model;
        }
        public ICollection<ValueToProduct> ValueToProductList()
        {


            return ValueToProductRepository.Get(null, ks => ks.Product, ks => ks.Value).Result.ToList();
        }
    }

}