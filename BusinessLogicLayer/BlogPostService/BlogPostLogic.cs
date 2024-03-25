using DataAccessLayer.Models;
using BusinessLogicLayer.TagService;
using BusinessLogicLayer.TagToBlogPostService;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.CommentService;

namespace BusinessLogicLayer.BlogPostService
{
    public class BlogPostLogic
    {
        private readonly MainRepository<BlogPost> blogRepository;
        private readonly CommentLogic commentLogic;
        private readonly TagToBlogPostLogic tagToBlogPostLogic;
        private readonly TagLogic tagLogic;
        public BlogPostLogic(MainRepository<BlogPost> blogRepository, CommentLogic commentLogic, TagToBlogPostLogic tagToBlogPostLogic, TagLogic tagLogic)
        {
            this.blogRepository = blogRepository;
            this.commentLogic = commentLogic;
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
            blogpost.Comments = commentLogic.CommentsOfPost(Id);
            foreach(var bt in blogpost.TagToBlogPosts)
            {
                bt.Tag = tagLogic.TagDetail(bt.Tag_Id);
            }
            //include tags and comments in future;
            return blogpost;
        }
        public BlogPost BlogPostDetail(string Title)
        {
            var blogpost = blogRepository.Get(bp => bp.Title == Title, b => b.TagToBlogPosts).Result.FirstOrDefault();
            blogpost.Comments = commentLogic.CommentsOfPost(blogpost.Id);
            foreach (var bt in blogpost.TagToBlogPosts)
            {
                bt.Tag = tagLogic.TagDetail(bt.Tag_Id);
            }
            //include tags and comments in future;
            return blogpost;
        }
        public List<BlogPost> blogPostList()
        {
            var blogpostList = blogRepository.Get().Result.ToList();
             foreach(var blogpost in blogpostList)
            {
                blogpost.Comments = commentLogic.CommentsOfPost(blogpost.Id);
                blogpost.TagToBlogPosts = tagToBlogPostLogic.TagToBlogPostList().Where(t => t.BlogPost_Id == blogpost.Id).ToList();
            }  
            return blogpostList;
        }
        public List<BlogPost> SearchPost(string search)
        {
            var postList = blogPostList().Where(p => p.Title.Contains(search) || p.TagToBlogPosts.Select(t => t.Tag.TagName).ToList().Contains(search)).ToList();
            return postList;
        }
    }
}
