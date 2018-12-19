
namespace GenericRepository.Repository
{
    using GenericRepository.Transaction;
    using GenericRepository.UnitOfWork;
    using Microsoft.EntityFrameworkCore.Query;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<T> where T : class
    {
        IUnitOfWork UnitOfWork { get; }

        //ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Snapshot);

        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] pathInclude);

        Task<T> GetAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] pathInclude);

        Task<T> GetAsync(Expression<Func<T, T>> selector,
                                          Expression<Func<T, bool>> predicate = null,
                                          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          Func<IQueryable<T>, IIncludableQueryable<T, object>> pathInclude = null,
                                          bool disableTracking = true);

        T Get(Guid id);

        Task<T> GetByIdAsync(Guid Id);

        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] pathInclude);

        IEnumerable<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] pathInclude);

        T Insert(T obj);

        T Update(T obj);

        void Remove(Guid id);

        void Remove(T obj);

        Task<T> GetAsyncAsNoTracking(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] pathInclude);

        Task<IEnumerable<T>> GetManyAsyncAsNoTracking(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] pathInclude);
    }
}
