using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Services.DatabaseAccessService;
using xsolla_revenue_calculator.Services.RevenueForecastService;
using xsolla_revenue_calculator.ViewModels;

namespace xsolla_revenue_calculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RevenueForecastController : Controller
    {
        private readonly IDatabaseAccessService _databaseAccessService;
        private readonly IRevenueForecastService _revenueForecastService;
        private readonly IMapper _mapper;

        public RevenueForecastController(IDatabaseAccessService databaseAccessService, IRevenueForecastService revenueForecastService, IMapper mapper)
        {
            _databaseAccessService = databaseAccessService;
            _revenueForecastService = revenueForecastService;
            _mapper = mapper;
        }

        /// <summary>
        /// Method to post user information to the service
        /// </summary>
        /// <param name="userInfo">user model</param>
        /// <response code="200">Returns in case of success</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PostUserInfoAsync([FromBody] UserInfo userInfo)
        {
            await _databaseAccessService.LogUserAsync(userInfo);
            var draftForecast = await _revenueForecastService.StartCalculationAsync(userInfo);
            return Ok(_mapper.Map<RevenueForecastViewModel>(draftForecast));
        }
    }
}