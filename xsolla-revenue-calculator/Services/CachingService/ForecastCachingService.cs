using System.Threading.Tasks;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.Services
{
    public class ForecastCachingService : IForecastCachingService
    {
        private readonly IRedisConnectionService _connectionService;

        public ForecastCachingService(IRedisConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task<string> GetRevenueForecastIdAsync(UserInfo userInfo)
        {
            return await Task.FromResult("");
        }
    }

    public interface IForecastCachingService
    {
        Task<string> GetRevenueForecastIdAsync(UserInfo userInfo);
    }
}