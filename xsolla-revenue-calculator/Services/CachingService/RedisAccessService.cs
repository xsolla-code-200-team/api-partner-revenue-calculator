namespace xsolla_revenue_calculator.Services.CachingService
{
    public class RedisConnectionService : IRedisConnectionService
    {
        public string Get(string key)
        {
            return "";
        }

        public void Set(string key, string value)
        {
            
        }
    }

    public interface IRedisConnectionService
    {
        void Set(string key, string value);
        string Get(string key);
    }
}