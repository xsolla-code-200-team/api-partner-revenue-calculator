using Microsoft.AspNetCore.Mvc;

namespace xsolla_revenue_calculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RevenueForecastController : Controller
    {

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}