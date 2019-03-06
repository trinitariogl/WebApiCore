
namespace UnitOfWork.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using ApplicationServiceInterfaces.Models;
    using ApplicationServiceInterfaces.Services;
    using CrossCutting.Utils.CryptoService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    //[ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    [Route("api/[controller]")]
    [Authorize]
    public class ValuesController : Controller
    {
        private IUserAccountApplicationService _userAccountApplicationService;
        private readonly ILogger<ValuesController> _logger;
        private readonly IConfiguration _configuration;

        public ValuesController(IConfiguration configuration, IUserAccountApplicationService userAccountApplicationService, ILogger<ValuesController> logger)
        {
            this._userAccountApplicationService = userAccountApplicationService;
            this._logger = logger;
            this._configuration = configuration;
        }

        /// <summary>
        /// LogIn User
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Token</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LogOnDto model)
        {
            JwtSecurityToken token = null;

            if (ModelState.IsValid)
            {
                token = await _userAccountApplicationService.Authenticate(model);

                if (User.IsInRole("Administrator") || User.IsInRole("Readers") || User.IsInRole("CallCenter"))
                {
                    return RedirectToAction("Index");
                }
                
                if (User.IsInRole("Public"))
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Error");
            }

            return Ok(
                new
                {
                    response = new JwtSecurityTokenHandler().WriteToken(token)
                }
            );

        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Status Code Http</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(RegisterDto model)
        {
            IEnumerable<Claim> claims = User.Claims;

            if (ModelState.IsValid)
            {
                UserAccountDto userDto = MappingUser(model);

                await this._userAccountApplicationService.CreateUser(userDto);
            }
            else
            {
                //El CrateUser nos devolvería un SecurityResult, para el caso de una web con mvc
                //AddErrors(result);
                return StatusCode(422);
            }

            return Ok();

        }
    

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody]UserAccountDto model)
        {
            if(ModelState.IsValid)
            {
                await this._userAccountApplicationService.UpdateUser(model);
            }
            else
            {
                return StatusCode(422);
            }

            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await this._userAccountApplicationService.DeleteUser(id);

            return Ok();
        }


        #region Private Methods

        //Método utilizado en Web para añadir los errores producidos al modelo
        private void AddErrors(SecurityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        //Mapeo de la entidad user
        private static UserAccountDto MappingUser(RegisterDto model)
        {
            UserAccountDto userDto = new UserAccountDto();
            userDto.Id = Guid.NewGuid().ToString();
            userDto.Username = model.Username;
            userDto.Email = model.Email;
            var salt = Crypto.CreateSalt(8);
            userDto.Salt = salt;
            userDto.PasswordHash = Crypto.GetSHA256Hash(model.Password, salt);
            userDto.Active = false;
            userDto.VerificationToken = Guid.NewGuid();

            return userDto;
        }

        #endregion
    }
}
