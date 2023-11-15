using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.services
{
    public class MainRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext context;
        public MainRepository(AppDbContext context)
        {
           this.context = context;
        }
        public void AddItem (TEntity entity)
        {
            context.Add(entity);
            context.SaveChanges();
        }
        public void EditItem(TEntity entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }
        public void DeleteItem(TEntity entity)
        {
            context.Remove(entity);
            context.SaveChanges();
        }
        public void DeleteItem(object Id)
        {
           var entity = context.Find(Id.GetType());
            DeleteItem(entity);
        }
        public List<TEntity> GetAllItem()
        {
            return context.Set<TEntity>().ToList();
        }

    }

}
