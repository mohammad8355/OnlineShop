using DataAccessLayer.Models;
using BusinessLogicLayer.TagToBlogPostService;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.TagService
{
    public class TagLogic
    {
        private readonly MainRepository<Tag> tagRepository;
        private readonly TagToBlogPostLogic tagToBlogPostLogic;
        public TagLogic(MainRepository<Tag> tagRepository, TagToBlogPostLogic tagToBlogPostLogic)
        {
            this.tagRepository = tagRepository;
            this.tagToBlogPostLogic = tagToBlogPostLogic;
        }
        public async Task<bool> AddTag(Tag model)
        {
            if (string.IsNullOrEmpty(model.TagName))
            {
                return false;
            }
            else
            {
                if(!tagRepository.Get(t => t.TagName == model.TagName).Any())
                {
                    await tagRepository.AddItem(model);
                    return true;
                }
                return false;
            }
        }
        public bool EditTag(Tag model)
        {
            if (string.IsNullOrEmpty(model.TagName))
            {
                return false;
            }
            else
            {
                if (!tagRepository.Get(t => t.TagName == model.TagName).Any())
                {
                    tagRepository.EditItem(model);
                    return true;
                }
                return true;
            }
        }
        public async Task<bool> DeleteTag(int Id)
        {
            if(tagToBlogPostLogic.TagToBlogPostList().Where(tp => tp.Tag_Id == Id).Any())
            {
                foreach (var tagToBlogPost in tagToBlogPostLogic.TagToBlogPostList().Where(tp => tp.Tag_Id == Id).ToList())
                {
                    if (!await tagToBlogPostLogic.DeleteTagToBlogPost(tagToBlogPost.BlogPost_Id, tagToBlogPost.Tag_Id))
                    {
                        return false;
                    }
                }
            }
            if (await tagRepository.DeleteItem(Id))
            {
                return true;
            }
            return false;
        }
        public Tag TagDetail(int Id)
        {
            var Tag = tagRepository.Get(bp => bp.Id == Id, t => t.tagToBlogPosts).FirstOrDefault();
            //include tags and comments in future;
            return Tag;
        }
        public Tag TagDetail(string TagName)
        {
            var Tag = tagRepository.Get(t => t.TagName == TagName,t => t.tagToBlogPosts).FirstOrDefault();
            //include tags and comments in future;
            return Tag;
        }
        public List<Tag> TagList()
        {
            var TagList = tagRepository.Get(null,t => t.tagToBlogPosts).ToList();
            return TagList;
        }
    }
}
