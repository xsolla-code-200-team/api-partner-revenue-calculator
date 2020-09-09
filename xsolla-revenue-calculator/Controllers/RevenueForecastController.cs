using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Services.ModelController;
using xsolla_revenue_calculator.Services.UserLoggingService;

namespace xsolla_revenue_calculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RevenueForecastController : Controller
    {
        private readonly IUserLoggingService _userLoggingService;
        private readonly IModelController _modelController;

        public RevenueForecastController(IUserLoggingService userLoggingService, IModelController modelController)
        {
            _userLoggingService = userLoggingService;
            _modelController = modelController;
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
            _modelController.Publish((await user).Email);
            return Ok(user);
        }
    }
}