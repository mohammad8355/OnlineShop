using BusinessEntity;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.BlogPostService
{
    public class BlogPostLogic
    {
        private readonly MainRepository<BlogPost> blogRepository;
        public BlogPostLogic(MainRepository<BlogPost> blogRepository)
        {
            this.blogRepository = blogRepository;
        }
        public async Task<bool> AddBlogPost(BlogPost model)
        {
            if(string.IsNullOrEmpty(model.CoverLink) || string.IsNullOrEmpty(model.Content) || string.IsNullOrEmpty(model.Title) || model.ReadingTime == 0 || string.IsNullOrEmpty(model.Author))
            {
                return false;
            }
            else
            {
               await blogRepository.AddItem(model);
                return true;
            }
        }
        public bool EditBlogPost(BlogPost model)
        {
            if (string.IsNullOrEmpty(model.CoverLink) || string.IsNullOrEmpty(model.Content) || string.IsNullOrEmpty(model.Title) || model.ReadingTime == 0 || string.IsNullOrEmpty(model.Author))
            {
                return false;
            }
            else
            {
                 blogRepository.EditItem(model);
                return true;
            }
        }
        public async Task<bool> DeleteBlogPost(int Id)
        {
            if(await blogRepository.DeleteItem(Id))
            {
                return true;
            }
            return false;
        }
        public BlogPost BlogPostDetail(int Id)
        {
            var blogpost = blogRepository.Get(bp => bp.Id == Id).Result.FirstOrDefault();
            //include tags and comments in future;
            return blogpost;
        }
        public List<BlogPost> blogPostList()
        {
            var blogpostList = blogRepository.Get().Result.ToList();
            return blogpostList;
        }

    }
}
