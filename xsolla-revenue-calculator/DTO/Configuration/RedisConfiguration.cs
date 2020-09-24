namespace xsolla_revenue_calculator.DTO.Configuration
{
    public class RedisConfiguration : IRedisConfiguration
    {
        public string Host { get; set; }
        public string Port { get; set; }
    }

    public interface IRedisConfiguration
    {
        public string Host { get; set; }
        public string Port { get; set; }
    }
}