
namespace ApplicationServiceLayer.Services
{
    using ApplicationServiceInterfaces.Converters;
    using ApplicationServiceInterfaces.Models;
    using ApplicationServiceInterfaces.Services;
    using DataServiceInterfaces.Models;
    using DataServiceInterfaces.Services;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;
    using Newtonsoft.Json;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using System.IdentityModel.Tokens.Jwt;
    using System;
    using Microsoft.Extensions.Configuration;
    using CrossCutting.Utils.CryptoService;
    using System.Linq;

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
            //UserAccounts userAccount = await this.userAccountDataService.GetAsync(x => x.Username == username);

            UserAccounts userAccount = await this.userAccountDataService.GetAsync(x => x.Username == username, x => x.UserRoles);

            UserAccounts userAccount2 = await this.userAccountDataService.GetAsync(null,
                                                                            predicate: b => b.Username == username,
                                                                            pathInclude: source =>
                                                                                         source.Include(a => a.UserRoles)
                                                                                               .ThenInclude(a => a.Rol));

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

        public async Task<UserAccountDto> UpdateUser(UserAccountDto updateUser)
        {
            updateUser = MappingUserUpdate(updateUser);

            UserAccounts user = userAccountConverter.MapUserAccount(updateUser);

            user = this.userAccountDataService.Update(user);

            var commited = await userAccountDataService.UnitOfWork.SaveEntitiesAsync();

            return userAccountConverter.MapUserAccountDto(user);
        }

        public async Task<bool> DeleteUser(string id)
        {
            UserAccounts user = await this.userAccountDataService.GetByIdAsync(Guid.Parse(id));

            if(user == null)
            {
                throw new Exception();
            }

            this.userAccountDataService.Remove(Guid.Parse(id));

            return true;

        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="userLogOn"></param>
        /// <returns>Token</returns>
        public async Task<JwtSecurityToken> Authenticate(LogOnDto userLogOn)
        {
            UserAccountDto user = await this.FindUserByUsername(userLogOn.Username);

            if (user == null)
            {
                throw new System.Security.Authentication.InvalidCredentialException();
            }

            var hash = Crypto.GetHashedPassword(user.Salt, userLogOn.Password);

            if (!hash.SequenceEqual(user.PasswordHash))
            {
                throw new System.Security.Authentication.InvalidCredentialException();
            }

            return GenerateToken(user);
        }

        /// <summary>
        /// Generate Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Token</returns>
        private static JwtSecurityToken GenerateToken(UserAccountDto user)
        {
            var config = new ConfigurationBuilder()
                                        .AddJsonFile("appsettings.json")
                                        .Build();

            var secretKey = config["ApiAuth:SecretKey"];

            var claims = new[]
            {
                new Claim("UserData", JsonConvert.SerializeObject(user)),
                new Claim("id", user.Id),
                new Claim("email", user.Email),
                new Claim("name", user.Username),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Generamos el Token
            var token = new JwtSecurityToken
            (
                issuer: config["ApiAuth:Issuer"],
                audience: config["ApiAuth:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(60),
                notBefore: DateTime.UtcNow,
                signingCredentials: creds
            );

            return token;
        }

        //Mapeo de la entidad user
        //Añadir el id en la creación de la entidad.
        private static UserAccountDto MappingUserUpdate(UserAccountDto model)
        {
            UserAccountDto userDto = new UserAccountDto();
            //userDto.Id = Guid.NewGuid().ToString();
            userDto.Username = model.Username;
            userDto.Email = model.Email;
            var salt = Crypto.CreateSalt(8);
            userDto.Salt = salt;
            userDto.PasswordHash = Crypto.GetSHA256Hash(model.Password, salt);
            userDto.Active = false;
            userDto.VerificationToken = Guid.NewGuid();

            return userDto;
        }
    }
}
