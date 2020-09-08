using System;
using System.Threading.Tasks;
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

        [HttpPost]
        public async Task<ActionResult<UserInfo>> PostUserInfo([FromBody] UserInfoRequestBody userInfoRequestBody)
        {
            var user = _userLoggingService.LogUser(userInfoRequestBody);
            return Ok(await user);
        }
    }
}