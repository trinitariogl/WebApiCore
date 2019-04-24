
namespace GenericRepository.Repository
{
    using GenericRepository.Transaction;
    using GenericRepository.UnitOfWork;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public abstract class Repository<T> : IRepository<T> where T : class //UnitOfWork, IRepository<T> where T : class
    {
        protected DbContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return (IUnitOfWork)_context;
            }
        }

        public Repository(DbContext context) //: base(context)
        {
            _context = context;
        }

        /*public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Snapshot)
        {
            return new Transaction(_context.Database.BeginTransaction(isolationLevel));
        }*/

        protected DbSet<T> Set
        {
            get
            {
                return _context.Set<T>();
            }
        }

        public T Insert(T obj)
        {
            Set.Add(obj);
            return obj;
        }


        public void Remove(Guid id)
        {
            T obj = Get(id);
            Remove(obj);
        }


        public void Remove(T obj)
        {
            Set.Remove(obj);
        }

        public T Update(T obj)
        {
            Set.Attach(obj);

            var entry = _context.Entry(obj);

            entry.State = EntityState.Modified;

            return obj;
        }

        public T Get(Guid id)
        {
            return this.Set.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Set.AsEnumerable();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Set.ToListAsync();
        }

        public T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] pathInclude)
        {
            IQueryable<T> dbQuery = Set;
            if (pathInclude != null)
            {
                foreach (var item in pathInclude)
                {
                    dbQuery = dbQuery.Include(item);
                }
            }
            return dbQuery.Where(where).FirstOrDefault<T>();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] pathInclude)
        {
            IQueryable<T> dbQuery = Set;
            
            if (pathInclude != null)
            {
                foreach (var item in pathInclude)
                {
                    dbQuery = dbQuery.Include(item);
                }
            }
            return await dbQuery.Where(where).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, T>> selector = null,
                                          Expression<Func<T, bool>> predicate = null,
                                          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                          Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                          bool disableTracking = true)
        {
            T result = null;
            IQueryable<T> dbQuery = Set;

            if (disableTracking)
            {
                dbQuery = dbQuery.AsNoTracking();
            }

            if (include != null)
            {
                dbQuery = include(dbQuery);
            }

            if (predicate != null)
            {
                dbQuery = dbQuery.Where(predicate);
            }

            if (orderBy != null)
            {
                if (selector != null)
                {
                    result = await orderBy(dbQuery).Select(selector).FirstOrDefaultAsync();
                }
                else
                {
                    result = await orderBy(dbQuery).FirstOrDefaultAsync();
                }
            }
            else
            {
                if (selector != null)
                {
                    result = await dbQuery.Select(selector).FirstOrDefaultAsync();
                }
                else
                {
                    result = await dbQuery.FirstOrDefaultAsync();
                }
            }

            return result;
        }

        public T GetById(Guid Id)
        {
            return Set.Find(Id);
        }

        public async Task<T> GetByIdAsync(Guid Id)
        {
            return await Set.FindAsync(Id);
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] pathInclude)
        {
            IQueryable<T> dbQuery = Set;
            if (pathInclude != null)
                foreach (var item in pathInclude)
                {
                    dbQuery = dbQuery.Include(item);
                }

            var result = await dbQuery.Where(where).ToListAsync();
            return result;
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] pathInclude)
        {
            IQueryable<T> dbQuery = Set;
            if (pathInclude != null)
                foreach (var item in pathInclude)
                {
                    dbQuery = dbQuery.Include(item);
                }

            var result = dbQuery.Where(where).ToList();
            return result;
        }

        public async Task<T> GetAsyncAsNoTracking(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] pathInclude)
        {
            IQueryable<T> dbQuery = Set;
            if (pathInclude != null)
            {
                foreach (var item in pathInclude)
                {
                    dbQuery = dbQuery.Include(item);
                }
            }
            return await dbQuery.AsNoTracking().Where(where).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetManyAsyncAsNoTracking(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] pathInclude)
        {
            IQueryable<T> dbQuery = Set;
            if (pathInclude != null)
            {
                foreach (var item in pathInclude)
                {
                    dbQuery = dbQuery.Include(item);
                }
            }
            return await dbQuery.AsNoTracking().Where(where).ToListAsync();
        }
    }
}
