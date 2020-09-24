using System.Threading.Tasks;
using AutoMapper;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.Services.CachingService
{
    public class ForecastCachingService : IForecastCachingService
    {
        private readonly IRedisAccessService _accessService;
        private readonly IHashingService _hashingService;
        private readonly IMapper _mapper;
        private readonly IDatabaseAccessService _databaseAccessService;

        public ForecastCachingService(IRedisAccessService accessService, IMapper mapper, IHashingService hashingService, IDatabaseAccessService databaseAccessService)
        {
            _accessService = accessService;
            _mapper = mapper;
            _hashingService = hashingService;
            _databaseAccessService = databaseAccessService;
        }

        public async Task<RevenueForecasts?> GetRevenueForecastAsync(UserInfo userInfo)
        {
            var cashedUserInfo = _mapper.Map<CachedUserInfo>(userInfo);
            var hash = _hashingService.GetHash(cashedUserInfo);
            var forecastId = await _accessService.GetAsync(hash);
            if (forecastId == null) return null;
            
            var actualUserInfo = await _databaseAccessService.GetUserInfoByForecastId(forecastId);
            var actualCachedUserInfo = _mapper.Map<CachedUserInfo>(actualUserInfo);
            if (actualCachedUserInfo.Equals(cashedUserInfo))
                return await _databaseAccessService.GetForecastAsync(forecastId);
            return null;
        }

        public async Task AddForecastToCache(RevenueForecasts forecasts)
        {
            var userInfo = await _databaseAccessService.GetUserInfoByForecastId(forecasts.Id.ToString());
            var cashedUserInfo = _mapper.Map<CachedUserInfo>(userInfo);
            var hash = _hashingService.GetHash(cashedUserInfo);
            await _accessService.SetAsync(hash, forecasts.Id.ToString());
        }
    }

    public interface IForecastCachingService
    {
        Task<RevenueForecasts?> GetRevenueForecastAsync(UserInfo userInfo);
        Task AddForecastToCache(RevenueForecasts forecasts);
    }
}