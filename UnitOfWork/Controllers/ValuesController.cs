
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

        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> Get()
        {
            UserAccountDto user = await this._userAccountApplicationService.FindUserById("1ea57e74-6984-4d98-86b3-c516c464b356");

            UserAccountDto userAccountDto = new UserAccountDto();

            _logger.LogInformation("Log Information");

            userAccountDto.Username = "TNT44";
            userAccountDto.Id = Guid.NewGuid().ToString();
            userAccountDto.Email = "trinogl@hotmail.com";
            userAccountDto.PrefferedLanguage = "es";
            var salt = Crypto.CreateSalt(8);
            userAccountDto.Salt = salt;
            userAccountDto.PasswordHash = Crypto.GetSHA256Hash(Guid.NewGuid().ToString(), salt);
            userAccountDto.Active = false;
            userAccountDto.VerificationToken = Guid.NewGuid();

            return "value";
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Status Code Http</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(RegisterDto model)
        {
            if(ModelState.IsValid)
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

        // GET api/values/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("/user/{id}")]
        [Authorize(Roles = "User")]
        public string GetUser(int id)
        {
            IEnumerable<Claim> claims = User.Claims;

            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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
            userDto.PasswordHash = Crypto.GetSHA256Hash(userDto.Id, salt);
            userDto.Active = false;
            userDto.VerificationToken = Guid.NewGuid();

            return userDto;
        }

        #endregion
    }
}
