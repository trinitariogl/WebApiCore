
namespace ApplicationServiceLayer.Converters
{
    using ApplicationServiceInterfaces.Converters;
    using ApplicationServiceInterfaces.Models;
    using DataServiceInterfaces.Models;
    using Omu.ValueInjecter;

    public class UserAccountConverter: IUserAccountConverter
    {
        public UserAccountDto MapUserAccountDto(UserAccounts userAccount)
        {
            if (userAccount == null) { return null; }
            var userDto = System.Activator.CreateInstance<UserAccountDto>();
            userDto.InjectFrom(userAccount);
            return userDto;
        }

        public UserAccounts MapUserAccount(UserAccountDto userAccountDto)
        {
            if (userAccountDto == null) { return null; }
            var user = System.Activator.CreateInstance<UserAccounts>();
            user.InjectFrom(userAccountDto);
            return user;
        }
    }
}
