
namespace UnitOfWork.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ApplicationServiceInterfaces.Models;
    using ApplicationServiceInterfaces.Services;
    using CrossCutting.Utils.CryptoService;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    //[ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private IUserAccountApplicationService _userAccountApplicationService;
        private readonly ILoggerFactory _logger;

        public ValuesController(IUserAccountApplicationService userAccountApplicationService, ILoggerFactory logger)
        {
            this._userAccountApplicationService = userAccountApplicationService;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            UserAccountDto user = await this._userAccountApplicationService.FindUserById("1ea57e74-6984-4d98-86b3-c516c464b356");

            UserAccountDto userAccountDto = new UserAccountDto();

            var logger = _logger.CreateLogger("LoggerCategory");
            logger.LogInformation("Calling the ping action");

            userAccountDto.Username = "TNT44";
            userAccountDto.Id = Guid.NewGuid().ToString();
            userAccountDto.Email = "trinogl@hotmail.com";
            userAccountDto.PrefferedLanguage = "es";
            var salt = Crypto.CreateSalt(8);
            userAccountDto.Salt = salt;
            userAccountDto.PasswordHash = Crypto.GetSHA256Hash(Guid.NewGuid().ToString(), salt);
            userAccountDto.Active = false;
            userAccountDto.VerificationToken = Guid.NewGuid();

            UserAccountDto userAccountNew = await _userAccountApplicationService.CreateUser(userAccountDto);

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
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
    }
}
