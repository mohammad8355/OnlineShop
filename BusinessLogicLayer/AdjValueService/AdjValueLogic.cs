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
        public AdjValueLogic(MainRepository<AdjValue> AdjValueRepository, MainRepository<AdjKey> AdjKeyRepository)
        {
            this.AdjKeyRepository = AdjKeyRepository;
            this.AdjValueRepository = AdjValueRepository;
        }
        public async Task<bool> AddAdjValue(AdjValue model)
        {
            if(model == null || model.adjkey_Id == 0)
            {
                return false;
            }
            else
            {
               var adjKey = AdjKeyRepository.Get(k => k.Id == model.adjkey_Id).Result.FirstOrDefault();
                model.adjKey = adjKey;
               await AdjValueRepository.AddItem(model);
                return true;
            }
        }
        public async Task<bool> EditAdjValue(AdjValue model)
        {
            if (model == null || model.adjkey_Id == 0)
            {
                return false;
            }
            else
            {
                var adjKey = AdjKeyRepository.Get(k => k.Id == model.adjkey_Id).Result.FirstOrDefault();
                model.adjKey = adjKey;
                 AdjValueRepository.EditItem(model);

                return true;
            }
        }
        public async Task<bool> DeleteAdjValue(int Id)
        {
            if(await AdjValueRepository.DeleteItem(Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public AdjValue AdjValueDetail(int Id)
        {
                var model = AdjValueRepository.Get(a => a.Id == Id).Result.FirstOrDefault();
                    model.adjKey = AdjKeyRepository.Get(k => k.Id == model.adjkey_Id).Result.FirstOrDefault();
                    return model;
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
