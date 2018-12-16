namespace DataServiceLayer.Context
{
    using GenericRepository.UnitOfWork;
    using Microsoft.EntityFrameworkCore;
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
    }
}
