
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
    public class ValuesController : ControllerBase
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
        [HttpPost("LogIn")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LogOnDto model)
        {
            JwtSecurityToken token = null;
            _logger.LogInformation("Mensae");
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
                await this._userAccountApplicationService.CreateUser(model);
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
        [HttpDelete("Delete/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(string id)
        {
            await this._userAccountApplicationService.DeleteUser(id);

            return Ok();
        }

        // DELETE api/values/Get/5
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string id)
        {
            //await this._userAccountApplicationService.DeleteUser(id);

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

        #endregion
    }
}
