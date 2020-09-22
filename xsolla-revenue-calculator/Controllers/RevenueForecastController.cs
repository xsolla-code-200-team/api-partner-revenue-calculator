using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xsolla_revenue_calculator.Exceptions;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Services;
using xsolla_revenue_calculator.Services.DatabaseAccessService;
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
        /// Posting user information to the service
        /// </summary>
        /// <param name="userInfo">user model</param>
        /// <response code="200">Returns in case of success</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(RevenueForecastViewModel))]
        public async Task<IActionResult> PostUserInfoAsync([FromBody] UserInfo userInfo)
        {
            if (!ModelState.IsValid) throw new ValidationException(ModelState);
            await _databaseAccessService.LogUserAsync(userInfo);
            var draftForecast = await _revenueForecastService.StartCalculationAsync(userInfo);
            return Ok(_mapper.Map<RevenueForecastViewModel>(draftForecast));
        }
        /// <summary>
        /// Getting information about the forecast with given id
        /// </summary>
        /// <param name="id">id of the forecast</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(RevenueForecastViewModel))]
        public async Task<IActionResult> GetForecast(string id)
        {
            var forecast = await _databaseAccessService.GetForecastAsync(id);
            return Ok(_mapper.Map<RevenueForecastViewModel>(forecast));
        }
    }
}