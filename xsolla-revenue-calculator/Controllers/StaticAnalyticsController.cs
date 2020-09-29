using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using xsolla_revenue_calculator.Exceptions;
using xsolla_revenue_calculator.Models.ForecastModels;
using xsolla_revenue_calculator.Models.StaticAnalyticsModels;
using xsolla_revenue_calculator.Models.UserInfoModels;
using xsolla_revenue_calculator.Services;

namespace xsolla_revenue_calculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StaticAnalyticsController : Controller
    {
        private readonly IStaticAnalyticsService _staticAnalyticsService;
        private readonly IMapper _mapper;

        public StaticAnalyticsController(IStaticAnalyticsService staticAnalyticsService, IMapper mapper)
        {
            _staticAnalyticsService = staticAnalyticsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting static information about given genre
        /// </summary>
        /// <response code="200">Returns in case of success</response>
        [HttpGet("{genre}")]
        [SwaggerResponse(200, type: typeof(GenreInfoViewModel))]
        [SwaggerResponse(400, type: typeof(ExceptionDetails))]
        public async Task<IActionResult> PostUserSimpleAsync(string genre)
        {
            var info = await _staticAnalyticsService.GetGenreInfo(genre);
            return Ok(_mapper.Map<GenreInfoViewModel>(info));
        }
    }
}