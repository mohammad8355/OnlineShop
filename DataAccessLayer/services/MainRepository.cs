using DataAccessLayer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.services
{
    public class MainRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext context;
        private DbSet<TEntity> _dbSet;
        public MainRepository(AppDbContext context)
        {
           this.context = context;
            _dbSet = context.Set<TEntity>();
        }
        public async Task AddItem(TEntity entity)
        {
           await context.AddAsync(entity);
            context.SaveChanges();
        }
        public async Task EditItem(TEntity entity)
        {
            context.Update(entity);
         await context.SaveChangesAsync();
        }
        private async Task DeleteItem(TEntity entity)
        {
            context.Remove(entity);
           await context.SaveChangesAsync();
        }
        public async Task DeleteItem(object Id)
        {
           var entity = context.Find(Id.GetType());
             await DeleteItem(entity);
        }
        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity,bool>>? where = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (where != null)
            {
                query = query.Where(where);
            }
            return await query.ToListAsync();
        }

    }

}
