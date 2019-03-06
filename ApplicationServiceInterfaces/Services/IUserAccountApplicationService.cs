

namespace ApplicationServiceInterfaces.Services
{
    using ApplicationServiceInterfaces.Models;
    using System.IdentityModel.Tokens.Jwt;
    using System.Threading.Tasks;

    public interface IUserAccountApplicationService
    {
        Task<UserAccountDto> FindUserByUsername(string username);

        Task<UserAccountDto> FindUserById(string id);

        Task<UserAccountDto> FindUserByEmail(string email);

        Task<UserAccountDto> CreateUser(UserAccountDto newUser);

        Task<JwtSecurityToken> Authenticate(LogOnDto userLogon);

        Task<UserAccountDto> UpdateUser(UserAccountDto updateUser);

        Task<bool> DeleteUser(string id);
    }
}
