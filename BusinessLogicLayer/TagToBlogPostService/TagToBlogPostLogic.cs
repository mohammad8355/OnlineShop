using BusinessEntity;
using DataAccessLayer.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.TagToBlogPostService
{
    public class TagToBlogPostLogic
    {
        private readonly MainRepository<TagToBlogPost> tagToBlogPostRepository;
        public TagToBlogPostLogic(MainRepository<TagToBlogPost> tagToBlogPostRepository)
        {
            this.tagToBlogPostRepository = tagToBlogPostRepository;
        }
        public async Task<bool> AddTagToBlogPost(TagToBlogPost model)
        {
            if (model == null || model.Tag_Id == 0 || model.BlogPost_Id == 0)
            {
                return false;
            }
            else
            {
                if (!tagToBlogPostRepository.Get(ks => ks.BlogPost_Id == model.BlogPost_Id && ks.Tag_Id == model.Tag_Id).Result.Any())
                {
                    await tagToBlogPostRepository.AddItem(model);
                }
                return true;
            }
        }
        public async Task<bool> DeleteTagToBlogPost(int BlogPost_Id, int Tag_Id)
        {
            if (tagToBlogPostRepository.Get(s => s.Tag_Id == Tag_Id && s.BlogPost_Id == BlogPost_Id).Result.Any())
            {
                var TagToBlogPost = tagToBlogPostRepository.Get(s => s.Tag_Id == Tag_Id && s.BlogPost_Id == BlogPost_Id).Result.FirstOrDefault();
                await tagToBlogPostRepository.DeleteItem(TagToBlogPost); return true;
            }
            return false;
        }
        public TagToBlogPost TagToBlogPostDetail(int BlogPost_Id, int Tag_Id)
        {
            var model = tagToBlogPostRepository.Get(ks => ks.BlogPost_Id == BlogPost_Id && ks.Tag_Id == Tag_Id).Result.FirstOrDefault();
            return model;
        }
        public ICollection<TagToBlogPost> TagToBlogPostList()
        {


            return tagToBlogPostRepository.Get(null, ks => ks.Tag, ks => ks.BlogPost).Result.ToList();
        }
    }
}
