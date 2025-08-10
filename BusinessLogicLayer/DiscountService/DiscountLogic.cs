using DataAccessLayer.Models;

using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utility.DiscountCodeGenerator;

namespace BusinessLogicLayer.DiscountService
{
    public class DiscountLogic
    {
        private readonly MainRepository<Discount> DiscountRepository;
        private readonly MainRepository<DiscountToProduct> DiscountToProductRepository;
        private readonly CodeGenerator codeGenerator;
        public DiscountLogic(CodeGenerator codeGenerator, MainRepository<Discount> DiscountRepository, MainRepository<DiscountToProduct> DiscountToProductRepository)
        {
            this.DiscountRepository = DiscountRepository;
            this.DiscountToProductRepository = DiscountToProductRepository;
            this.codeGenerator = codeGenerator;
        }
        public async Task<bool> AddDiscount(Discount model)
        {
            if (!string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.DateBase) && model.Value != 0)
            {
                if (model.DiscountCode == null)
                {
                    var code = codeGenerator.GenerateDiscountCode(7);
                    model.DiscountCode = code;
                    Console.WriteLine(code);
                }
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
            if (DiscountRepository.Get(p => p.Id == Id).Any())
            {
                var discount = DiscountRepository.Get(d => d.Id == Id).First();
                discount.discountToProducts = DiscountToProductRepository.Get(d => d.Discount_Id == Id).ToList();
                return discount;
            }
            else
            {
                return new Discount();
            }
        }
        public async Task<Discount> DiscountDetails(string code)
        {
            if (DiscountRepository.Get(p => p.DiscountCode == code && p.Active == true).Any())
            {
                var result = await DiscountRepository.Get(d => d.DiscountCode == code && d.Active == true).ToListAsync();
                var discount = result.First();
                discount.discountToProducts = DiscountToProductRepository.Get(d => d.Discount_Id == discount.Id).ToList();
                return discount;
            }
            else
            {
                return new Discount();
            }
        }
        public (bool result , string message) IsValid(Discount discount)
        {
            var result = false;
            var message = "";
            if (discount != null)
            {
                if (discount.Active && discount.Value != 0)
                {
                    var now = DateTime.Now;
                    var duration = now - discount.CreateDate;
                    double discountDuration = 0;
                    switch (discount.DateBase)
                    {

                        case "minute":
                            discountDuration = discount.Duration * 1;
                            break;
                        case "day":
                            discountDuration = discount.Duration * 1440;
                            break;
                        case "week":
                            discountDuration = discount.Duration * 10080;
                            break;
                        case "mounth":
                            discountDuration = discount.Duration * 40320;
                            break;
                        case "year":
                            discountDuration = discount.Duration * 483840;
                            break;
                        default:
                            discountDuration = 0;
                            break;
                    }
                    if(duration.TotalMinutes < discountDuration)
                    {
                        result = true;
                        message = "کد تحفیف معتبر است";
                    }
                    else
                    {
                        message = "کد تخفیف منقضی شده است";
                    }
                }
                else
                {
                    message = "کد تخفیف صحیح نمی باشد";
                }
            }
            else
            {
                message = "کد تخفیف صحیح نمی باشد";
            }
            return (result, message);
        }
        public async Task<ICollection<Discount>> DiscountList()
        {
            ICollection<Discount> discountList = new List<Discount>();
            foreach (var item in DiscountRepository.Get().ToList())
            {
                var discountToProduct = DiscountToProductRepository.Get(d => d.Discount_Id == item.Id).ToList();
                item.discountToProducts = discountToProduct;
                discountList.Add(item);
            }
            return discountList;

        }

    }
}
