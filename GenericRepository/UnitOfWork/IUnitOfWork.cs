
namespace GenericRepository.UnitOfWork
{
    using GenericRepository.Transaction;
    using System;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
        ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Snapshot);

        /*ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Snapshot);
        void Commit();
        Task CommitAsync();
        void Attach<T>(T obj) where T : class;*/
    }
}
