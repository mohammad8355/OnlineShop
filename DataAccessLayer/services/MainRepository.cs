using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer.services
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
        public async Task<TEntity> AddItem(TEntity entity)
        {
            var model = await context.AddAsync(entity);
            context.SaveChanges();
            return model.Entity;    
        }
        public void EditItem(TEntity entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }
        public async Task DeleteItem(TEntity entity)
        {
            context.Remove(entity);
           await context.SaveChangesAsync();
        }
        public async Task<bool> DeleteItem(int Id)
        {
            var entity = context.Set<TEntity>().Find(Id);
            if(entity != null)
            {
                await DeleteItem(entity);
                return true;
            }
            else
            {
                return false;
            }
        }
       
        public IQueryable<TEntity> Get(Expression<Func<TEntity,bool>>? where = null,params Expression<Func<TEntity, object>>[]? includes)
        {
            IQueryable<TEntity> query = _dbSet;
            if (where != null)
            {
                query = query.Where(where);
            }
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }
        public async Task SaveChangeAsync()
        {
           await context.SaveChangesAsync();
        }
        public async Task Attach(TEntity entity)
        {
            context.Attach(entity);
           await SaveChangeAsync();
        }

    }

}
