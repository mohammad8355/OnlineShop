﻿using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
 

namespace BusinessLogicLayer.GeneralService
{
    public class GeneralLogic
    {
        private readonly MainRepository<General> GeneralRepository;
        public GeneralLogic(MainRepository<General> GeneralRepository)
        {
            this.GeneralRepository = GeneralRepository;
        }
        public async Task<bool> AddGeneral(General model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                return false;
            }
            else
            {
                await GeneralRepository.AddItem(model);
                return true;
            }
        }
        public async Task<bool> UpdateGeneral(General model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                return false;
            }
            else
            {
                 GeneralRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteGeneral(int Id)
        {
            if (await GeneralRepository.DeleteItem(Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<General> GeneralDetail(int Id)
        {
            if (GeneralRepository.Get(p => p.Id == Id).Result.Any())
            {
                var model = GeneralRepository.Get(p => p.Id == Id).Result.FirstOrDefault();
                return model;
            }
            else
            {
                return new General();
            }
        }
        public ICollection<General> GeneralList()
        {
            ICollection<General> Generals = new List<General>();
            foreach (var item in GeneralRepository.Get().Result.ToList())
            {

                Generals.Add(item);
            }
            return Generals;
        }
        public async Task<List<General>> ReturnByLabel(string label)
        {
            var ReturnByLabel = await GeneralRepository.Get(g => g.label == label);
            return ReturnByLabel.ToList();
        }
        public async Task<General> ReturnByName(string name)
        {
            var ReturnByName = await GeneralRepository.Get(g => g. Name == name);
            return ReturnByName.FirstOrDefault();
        }
    }

}
