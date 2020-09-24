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

        public ForecastCachingService(IRedisAccessService accessService, IMapper mapper, IHashingService hashingService)
        {
            _accessService = accessService;
            _mapper = mapper;
            _hashingService = hashingService;
        }

        public async Task<string> GetRevenueForecastIdAsync(UserInfo userInfo)
        {
            var cashedUserInfo = _mapper.Map<CachedUserInfo>(userInfo);
            var hash = _hashingService.GetHash(cashedUserInfo);
            var existingId = _accessService.Get(hash);
            return await Task.FromResult("");
        }
    }

    public interface IForecastCachingService
    {
        Task<string> GetRevenueForecastIdAsync(UserInfo userInfo);
    }
}