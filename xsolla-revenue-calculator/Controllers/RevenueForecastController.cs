using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Services.UserLoggingService;

namespace xsolla_revenue_calculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RevenueForecastController : Controller
    {
        private readonly IUserLoggingService _userLoggingService;

        public RevenueForecastController(IUserLoggingService userLoggingService)
        {
            _userLoggingService = userLoggingService;
        }
        /// <summary>
        /// Method to post user information to the service
        /// </summary>
        /// <param name="userInfoRequestBody">user model</param>
        /// <response code="200">Returns in case of success</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostUserInfoAsync([FromBody] UserInfoRequestBody userInfoRequestBody)
        {
            var user = _userLoggingService.LogUserAsync(userInfoRequestBody);
            return Ok(await user);
        }
    }
}