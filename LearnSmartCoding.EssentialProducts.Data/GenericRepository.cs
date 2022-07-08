using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace LearnSmartCoding.EssentialProducts.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, new()
    {
        private readonly EssentialProductsDbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(EssentialProductsDbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual Task<List<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", CancellationToken cancellationToken = default, bool trackable = true)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = trackable ? query.Where(filter).AsNoTracking() : query.Where(filter).AsNoTracking();
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty).AsNoTracking();
            }

            if (orderBy != null)
            {
                return orderBy(query).AsNoTracking().ToListAsync();
            }
            else
            {
                return query.AsNoTracking().ToListAsync();
            }
        }

        public virtual ValueTask<TEntity> GetByIdAsync(object id)
        {
            return dbSet.FindAsync(id);
        }

        public TEntity Add([NotNull] TEntity entity)
        {
            var entityEntryOfTEntity = dbSet.Add(entity);
            return entityEntryOfTEntity.Entity;
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void UpdateStateAlone(TEntity entityToUpdate)
        {
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public void DetachEntities(TEntity entityToDetach)
        {
            context.Entry(entityToDetach).State = EntityState.Detached;
        }

        public void DetachEntities(List<TEntity> entitiesToDetach)
        {
            foreach (var entity in entitiesToDetach)
            {
                context.Entry(entity).State = EntityState.Detached;
            }
        }



    }
}
