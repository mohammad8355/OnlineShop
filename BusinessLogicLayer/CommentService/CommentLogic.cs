using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Models;
using DataAccessLayer.services;

namespace BusinessLogicLayer.CommentService
{
    public class CommentLogic
    {
        private readonly MainRepository<Commnet> CommentRepository;
        public CommentLogic(MainRepository<Commnet> CommentRepository)
        {
            this.CommentRepository = CommentRepository;
        }
        public async Task<bool> AddComment(Commnet model)
        {
            if (string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.User_Id) || model.LastUpdate.Date != DateTime.Now.Date)
            {
                return false;
            }
            else
            {
                await CommentRepository.AddItem(model);
                return true;
            }
        }
        public bool EditComment(Commnet model)
        {
            if (string.IsNullOrEmpty(model.Description) || string.IsNullOrEmpty(model.User_Id) || model.LastUpdate.Date != DateTime.Now.Date)
            {
                return false;
            }
            else
            {
                CommentRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteComment(int Id)
        {
            if(await CommentRepository.DeleteItem(Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Commnet CommentDetail(int Id)
        {
            var comment =  CommentRepository.Get(c => c.Id == Id,c => c.User).Result.FirstOrDefault();
            return comment;
        }
        public List<Commnet> CommentList()
        {
            var comment = CommentRepository.Get(null, c => c.User ).Result.ToList();
            return comment;
        }

    }
}
