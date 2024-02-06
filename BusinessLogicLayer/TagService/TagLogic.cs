using BusinessEntity;
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
                if(!tagRepository.Get(t => t.TagName == model.TagName).Result.Any())
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
                if (!tagRepository.Get(t => t.TagName == model.TagName).Result.Any())
                {
                    tagRepository.EditItem(model);
                    return true;
                }
                return true;
            }
        }
        public async Task<bool> DeleteTag(int Id)
        {
            var tagtoblog = TagDetail(Id).tagToBlogPosts;
            foreach(var tagToBlogPost in tagtoblog)
            {
                if(!await tagToBlogPostLogic.DeleteTagToBlogPost(tagToBlogPost.BlogPost_Id, tagToBlogPost.Tag_Id))
                {
                    return false;
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
            var Tag = tagRepository.Get(bp => bp.Id == Id, t => t.tagToBlogPosts).Result.FirstOrDefault();
            //include tags and comments in future;
            return Tag;
        }
        public Tag TagDetail(string TagName)
        {
            var Tag = tagRepository.Get(t => t.TagName == TagName,t => t.tagToBlogPosts).Result.FirstOrDefault();
            //include tags and comments in future;
            return Tag;
        }
        public List<Tag> TagList()
        {
            var TagList = tagRepository.Get(null,t => t.tagToBlogPosts).Result.ToList();
            return TagList;
        }
    }
}
