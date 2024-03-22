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
            var isReplyExist = await CommentRepository.Get(c => c.Reply_Id == Id);
            var target = await CommentRepository.Get(c => c.Id == Id);
            if (target.Any())
            {
                if (isReplyExist.Any())
                {
                    var comments = isReplyExist.ToList();
                    foreach (var comment in comments)
                    {
                        comment.Reply_Id = null;
                        CommentRepository.EditItem(comment);
                    }
                }
            }
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
            var comment = CommentRepository.Get(null, c => c.User,c => c.Product).Result.ToList();
            return comment;
        }
        public List<Commnet> CommentsOfProduct(int Product_Id)
        {
            var comments = CommentList();
            var commentsOfProduct = comments.Where(c => c.Product_Id == Product_Id).ToList();
            return commentsOfProduct;
        }
        public List<Commnet> CommentsOfUser(string User_Id)
        {
            var comments = CommentList();
            var commentsOfUser = comments.Where(c => c.User_Id == User_Id).ToList();
            return commentsOfUser;
        }
        public List<Commnet> CommentsOfPost(int Post_Id)
        {
            var comments = CommentList();
            var commentsOfPost = comments.Where(c => c.BlogPost_Id == Post_Id).ToList();
            return commentsOfPost;
        }
    }
}
