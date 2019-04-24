
namespace UnitOfWork.Controllers
{
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using ApplicationServiceInterfaces.Models;
    using ApplicationServiceInterfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using System;

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
        [HttpGet("LogIn")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn([FromBody]LogOnDto model)
        {
            JwtSecurityToken token = null;

            if (ModelState.IsValid)
            {
                token = await _userAccountApplicationService.Authenticate(model);
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
        [HttpPost("CreateUser")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody]RegisterDto model)
        {
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
    
        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Estatus code Http</returns>
        [HttpPut("UpdateUser")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateUser([FromBody]UserAccountDto model)
        {
            IEnumerable<Claim> claims = User.Claims;
            JObject jObject = JObject.Parse(claims.FirstOrDefault().Value);
            Guid id = (Guid)jObject.SelectToken("Id");

            if (ModelState.IsValid)
            {
                await this._userAccountApplicationService.UpdateUser(id, model);
            }
            else
            {
                return StatusCode(422);
            }

            return Ok();
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status code Http</returns>
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Administrator")]
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

        #endregion
    }
}
