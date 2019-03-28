

namespace ApplicationServiceInterfaces.Services
{
    using ApplicationServiceInterfaces.Models;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Threading.Tasks;

    public interface IUserAccountApplicationService
    {
        Task<UserAccountDto> FindUserByUsername(string username);

        Task<UserAccountDto> FindUserById(Guid id);

        Task<UserAccountDto> FindUserByEmail(string email);

        Task<UserAccountDto> CreateUser(RegisterDto newUser);

        Task<JwtSecurityToken> Authenticate(LogOnDto userLogon);

        Task<UserAccountDto> UpdateUser(UserAccountDto updateUser);

        Task<bool> DeleteUser(string id);
    }
}
