namespace DataServiceLayer.Context
{
    using GenericRepository.Transaction;
    using GenericRepository.UnitOfWork;
    using Microsoft.EntityFrameworkCore;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class SecurityModelContext : DbContext, IUnitOfWork
    {
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync();
            return true;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return 0;
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return new Transaction(base.Database.BeginTransaction());
        }
    }
}
