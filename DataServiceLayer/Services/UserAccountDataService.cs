
namespace DataServiceLayer.Services
{
    using DataServiceInterfaces.Models;
    using DataServiceInterfaces.Services;
    using DataServiceLayer.Context;
    using GenericRepository.Repository;

    public class UserAccountDataService : Repository<UserAccounts>, IUserAccountDataService
    {
        public UserAccountDataService(SecurityModelContext context) : base(context)
        {

        }
    }
}
