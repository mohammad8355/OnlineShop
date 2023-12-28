using BusinessEntity;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.ReturnMultipleData;

namespace BusinessLogicLayer.AdjValueService
{
    public class AdjValueLogic
    {
        private readonly MainRepository<AdjValue> AdjValueRepository;
        private readonly MainRepository<AdjKey> AdjKeyRepository;
        private readonly ReturnMultipleData<AdjValue> returnMultipleData;
        public AdjValueLogic(MainRepository<AdjValue> AdjValueRepository, MainRepository<AdjKey> AdjKeyRepository, ReturnMultipleData<AdjValue> returnMultipleData)
        {
            this.returnMultipleData = returnMultipleData;
            this.AdjKeyRepository = AdjKeyRepository;
            this.AdjValueRepository = AdjValueRepository;
        }
        public async Task<List<object>> AddAdjValue(AdjValue model)
        {
            if(model == null || model.adjkey_Id == 0)
            {
                return await returnMultipleData.Return(false,"مشخصه مورد نظر یافت نشد و یا مدل خالی بوده است");
            }
            else
            {
               var adjKey = AdjKeyRepository.Get(k => k.Id == model.adjkey_Id).Result.FirstOrDefault();
                model.adjKey = adjKey;
               await AdjValueRepository.AddItem(model);
                return await returnMultipleData.Return(true,"عملیات افزودن با موفقیت انجام شد");
            }
        }
        public async Task<List<object>> EditAdjValue(AdjValue model)
        {
            if (model == null || model.adjkey_Id == 0)
            {
                return await returnMultipleData.Return(false, "مشخصه مورد نظر یافت نشد و یا مدل خالی بوده است");
            }
            else
            {
                var adjKey = AdjKeyRepository.Get(k => k.Id == model.adjkey_Id).Result.FirstOrDefault();
                model.adjKey = adjKey;
                 await AdjValueRepository.EditItem(model);

                return await returnMultipleData.Return(true, "عملیات ویرایش با موفقیت انجام شد");
            }
        }
        public async Task<List<object>> DeleteAdjValue(int Id)
        {
            if(await AdjValueRepository.DeleteItem(Id))
            {
                return await returnMultipleData.Return(true, "عملیات حذف با موفقیت انجام شد");
            }
            else
            {
                return await returnMultipleData.Return(false, "مقدار مورد نظر یافت نشد !");
            }
        }
        public async Task<List<object>> AdjValueDetail(int Id)
        {
            if(AdjValueRepository.Get(a => a.Id == Id).Result.Any())
            {
                var model = AdjValueRepository.Get(a => a.Id == Id).Result.FirstOrDefault();
                    model.adjKey = AdjKeyRepository.Get(k => k.Id == model.adjkey_Id).Result.FirstOrDefault();
                    return await returnMultipleData.Return(true,"مقدار مورد نظر یافت شد",model);
                
            }
            else
            {
                return await returnMultipleData.Return(false, "مقدار مورد نظر یافت نشد !"); ;
            }

        }
        public ICollection<AdjValue> AdjValueList()
        {
            ICollection<AdjValue> adjValues = new List<AdjValue>();
            foreach(var item in AdjValueRepository.Get().Result.ToList())
            {
                item.adjKey = AdjKeyRepository.Get(k => k.Id == item.adjkey_Id).Result.FirstOrDefault();
                adjValues.Add(item);
            }
            return adjValues;
        }
    }
}
