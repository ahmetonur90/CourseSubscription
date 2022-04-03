using CourseSubscription.Core.DTOs;
using CourseSubscription.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CourseSubscription.Core.Data
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        T Get(Expression<Func<T, bool>> filter);
        IQueryable<T> GetAll();
        IEnumerable<T> GetAllWithPagination(Expression<Func<T, bool>> predicate, PagingParamsDto param);
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        List<T> GetList(Expression<Func<T, bool>> filter = null);
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        Task<bool> Any(Expression<Func<T, bool>> filter);
        T GetById(int id);
    }
}
