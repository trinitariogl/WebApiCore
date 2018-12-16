
namespace ApplicationServiceLayer.Services
{
    using ApplicationServiceInterfaces.Converters;
    using ApplicationServiceInterfaces.Models;
    using ApplicationServiceInterfaces.Services;
    using DataServiceInterfaces.Models;
    using DataServiceInterfaces.Services;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class UserAccountApplicationService: IUserAccountApplicationService
    {
        private readonly IUserAccountDataService userAccountDataService = null;
        private readonly IUserAccountConverter userAccountConverter = null;

        public UserAccountApplicationService(IUserAccountDataService _userAccountDataService, IUserAccountConverter _userAccountConverter)
        {
            this.userAccountDataService = _userAccountDataService;
            this.userAccountConverter = _userAccountConverter;
        }


        public async Task<UserAccountDto> FindUserByUsername(string username)
        {
            UserAccounts userAccount = await this.userAccountDataService.GetAsync(x => x.Username == username);

            return userAccountConverter.MapUserAccountDto(userAccount);
        }

        public async Task<UserAccountDto> FindUserById(string id)
        {
            UserAccounts userAccount = await this.userAccountDataService.GetAsync(x => x.Id == id, x => x.UserRoles);

            UserAccounts userAccount2 = await this.userAccountDataService.GetAsync(null,
                                                                            predicate: b => b.Id == id,
                                                                            pathInclude: source => 
                                                                                         source.Include(a => a.UserRoles)
                                                                                               .ThenInclude(a => a.Rol));

            return userAccountConverter.MapUserAccountDto(userAccount);
        }

        public async Task<UserAccountDto> FindUserByEmail(string email)
        {

            UserAccounts userAccount = await this.userAccountDataService.GetAsync(x => x.Email == email);

            return userAccountConverter.MapUserAccountDto(userAccount);
        }

        public async Task<UserAccountDto> CreateUser(UserAccountDto newUser)
        {
            UserAccounts user = userAccountConverter.MapUserAccount(newUser);

            user = this.userAccountDataService.Insert(user);

            var commited = await userAccountDataService.UnitOfWork.SaveEntitiesAsync();

            return userAccountConverter.MapUserAccountDto(user);
        }
    }
}
