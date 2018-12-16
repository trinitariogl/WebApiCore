

namespace GenericRepository.UnitOfWork
{
    using System.Data;
    using System.Threading.Tasks;
    using GenericRepository.Transaction;
    using Microsoft.EntityFrameworkCore;

    //En este momento está inutilizado
    public class UnitOfWork //: IUnitOfWork
    {
        private DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Snapshot)
        {
            return new Transaction(_context.Database.BeginTransaction(isolationLevel));
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Attach<T>(T newUser) where T : class
        {
            var set = _context.Set<T>();
            set.Attach(newUser);
        }

        public void Dispose()
        {
            _context = null;
        }
    }
}
