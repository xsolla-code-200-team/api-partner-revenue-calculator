using System;
using System.Threading.Tasks;
using AutoMapper;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Models.ForecastModels;
using xsolla_revenue_calculator.Models.UserInfoModels;

namespace xsolla_revenue_calculator.Services.CachingService
{
    public class ForecastCachingService : IForecastCachingService
    {
        private readonly IRedisAccessService _redisAccessService;
        private readonly IHashingService _hashingService;
        private readonly IMapper _mapper;
        private readonly IDatabaseAccessService _databaseAccessService;

        private readonly TimeSpan _cacheExpiryTime = new TimeSpan(1, 0, 0, 0, 0);

        public ForecastCachingService(IRedisAccessService redisAccessService, IMapper mapper, IHashingService hashingService, IDatabaseAccessService databaseAccessService)
        {
            _redisAccessService = redisAccessService;
            _mapper = mapper;
            _hashingService = hashingService;
            _databaseAccessService = databaseAccessService;
        }

        public async Task<RevenueForecasts?> GetRevenueForecastAsync(FullUserInfo fullUserInfo)
        {
            var cashedUserInfo = _mapper.Map<CachedUserInfo>(fullUserInfo);
            var hash = _hashingService.GetHash(cashedUserInfo);
            var forecastId = await _redisAccessService.GetAsync(await hash);
            if (forecastId == null) return null;
            
            var actualUserInfo = await _databaseAccessService.GetUserInfoByForecastId(forecastId);
            var actualCachedUserInfo = _mapper.Map<CachedUserInfo>(actualUserInfo);
            if (actualCachedUserInfo.Equals(cashedUserInfo))
                return await _databaseAccessService.GetForecastAsync(forecastId);
            return null;
        }

        public async Task AddForecastToCacheAsync(RevenueForecasts forecasts)
        {
            var userInfo = await _databaseAccessService.GetUserInfoByForecastId(forecasts.Id.ToString());
            var cashedUserInfo = _mapper.Map<CachedUserInfo>(userInfo);
            var hash = _hashingService.GetHash(cashedUserInfo);
            await _redisAccessService.SetAsync(await hash, forecasts.Id.ToString(), _cacheExpiryTime);
        }
    }

    public interface IForecastCachingService
    {
        Task<RevenueForecasts?> GetRevenueForecastAsync(FullUserInfo fullUserInfo);
        Task AddForecastToCacheAsync(RevenueForecasts forecasts);
    }
}