using System.Threading.Tasks;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.Services.RevenueForecastService
{
    public interface IRevenueForecastService
    {
        Task<RevenueForecast> StartCalculationAsync(UserInfo userInfo);
    }
}