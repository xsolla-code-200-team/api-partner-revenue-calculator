using System.Threading.Tasks;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.Services.DatabaseAccessService
{
    public interface IDatabaseAccessService
    {
        Task<UserInfo> LogUserAsync(UserInfo userInfo);
        Task<RevenueForecast> PrepareForecastAsync();
        Task<RevenueForecast> GetRevenueForecast(string id);
        Task UpdateRevenueForecast(MessageFromModel message);

    }
}