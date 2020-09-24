using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using xsolla_revenue_calculator.DTO.Configuration;

namespace xsolla_revenue_calculator.Services.CachingService
{
    public class RedisAccessService : IRedisAccessService
    {
        private readonly string _redisConnectionString;
        private ConnectionMultiplexer _redis;
        
        private readonly IRedisConfiguration _redisConfiguration;

        public RedisAccessService(IRedisConfiguration redisConfiguration)
        {
            _redisConfiguration = redisConfiguration;
            _redisConnectionString = GetConnectionString();
            Connect();
        }

        private void Connect()
        {
            _redis = ConnectionMultiplexer.Connect(_redisConnectionString);
        }
        
        private string GetConnectionString()
        {
            var redisUrlString = Environment.GetEnvironmentVariable("REDIS_URL");
            if (string.IsNullOrEmpty(redisUrlString))
            {
                return $"{_redisConfiguration.Host}:{_redisConfiguration.Port}";
            }
            var redisUri = new Uri(redisUrlString);
            var userInfo = redisUri.UserInfo.Split(":");
            return $"{redisUri.Host}:{redisUri.Port}, password={userInfo[1]}";
        }

        public async Task<bool> SetAsync(string key, string value)
        {
            var db = _redis.GetDatabase();
            return await db.StringSetAsync(key, value);
        }
        
        public async Task<string?> GetAsync(string key)
        {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync(key);
            if (value.HasValue) return value;
            return null;
        }
    }

    public interface IRedisAccessService
    {
        Task<bool> SetAsync(string key, string value);
        Task<string?> GetAsync(string key);
    }
}