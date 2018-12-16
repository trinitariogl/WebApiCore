
namespace ApplicationServiceInterfaces.Converters
{
    using ApplicationServiceInterfaces.Models;
    using DataServiceInterfaces.Models;

    public interface IUserAccountConverter
    {
        UserAccountDto MapUserAccountDto(UserAccounts userAccount);

        UserAccounts MapUserAccount(UserAccountDto userAccountDto);
    }
}
