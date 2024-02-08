using DataAccessLayer.Models;
using BusinessLogicLayer.TagService;
using BusinessLogicLayer.TagToBlogPostService;
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
        private readonly TagToBlogPostLogic tagToBlogPostLogic;
        private readonly TagLogic tagLogic;
        public BlogPostLogic(MainRepository<BlogPost> blogRepository, TagToBlogPostLogic tagToBlogPostLogic, TagLogic tagLogic)
        {
            this.blogRepository = blogRepository;
            this.tagLogic = tagLogic;
            this.tagToBlogPostLogic = tagToBlogPostLogic;
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
            var blogpost = BlogPostDetail(Id);
            foreach (var tagtoblog in blogpost.TagToBlogPosts)
            {
                await tagToBlogPostLogic.DeleteTagToBlogPost(tagtoblog.BlogPost_Id, tagtoblog.Tag_Id);
            }
            if(await blogRepository.DeleteItem(Id))
            {
                return true;
            }
            return false;
        }
        public BlogPost BlogPostDetail(int Id)
        {
            var blogpost = blogRepository.Get(bp => bp.Id == Id,b => b.TagToBlogPosts).Result.FirstOrDefault();
            //include tags and comments in future;
            return blogpost;
        }
        public List<BlogPost> blogPostList()
        {
            var blogpostList = blogRepository.Get().Result.ToList();
             foreach(var blogpost in blogpostList)
            {
                blogpost.TagToBlogPosts = tagToBlogPostLogic.TagToBlogPostList().Where(t => t.BlogPost_Id == blogpost.Id).ToList();
            }  
            return blogpostList;
        }
    }
}
