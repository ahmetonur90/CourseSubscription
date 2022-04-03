using CourseSubscription.Core.DTOs;
using CourseSubscription.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CourseSubscription.Core.Data.EF
{
    public class EfEntityRepository<TEntity, TContext> : IEntityRepository<TEntity> where TEntity : class, IEntity, new() where TContext : DbContext, new()
    {
        public TEntity Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                var _entity = context.Entry(entity);
                _entity.State = EntityState.Added;
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var deneme = ex.Message.ToString();
                }

                return _entity.Entity;
            }
        }

        public void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                var _entity = context.Entry(entity);
                _entity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return filter == null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public TEntity Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                var _entity = context.Entry(entity);
                _entity.State = EntityState.Modified;
                context.SaveChanges();
                return _entity.Entity;
            }
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return await context.Set<TEntity>().Where(filter).AnyAsync();
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>();
            }
        }

        public IEnumerable<TEntity> GetAllWithPagination(Expression<Func<TEntity, bool>> predicate, PagingParamsDto param)
        {
            using (var context = new TContext())
            {
                return PagedList<TEntity>.ToPagedList(context.Set<TEntity>().Where(predicate),
                                    param.PageNumber,
                                    param.PageSize);
            }

        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            var context = new TContext();
            return context.Set<TEntity>().Where(predicate);
        }

        public TEntity GetById(int id)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().Find(id);
            }
        }
    }
}
