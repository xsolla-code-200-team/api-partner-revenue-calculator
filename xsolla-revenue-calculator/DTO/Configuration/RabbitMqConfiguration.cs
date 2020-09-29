namespace xsolla_revenue_calculator.DTO.Configuration
{
    public class RabbitMqConfiguration : IRabbitMqConfiguration
    {
        public string Host { get; set; }
        public RpcConnectionConfiguration ForecastRpcConfiguration { get; set; }

        public RpcConnectionConfiguration StaticInfoRpcConfiguration { get; set; }
    }

    public interface IRabbitMqConfiguration
    {
        public string Host { get; set; }
        
        public RpcConnectionConfiguration ForecastRpcConfiguration { get; set; }

        public RpcConnectionConfiguration StaticInfoRpcConfiguration { get; set; }
    }

    public class RpcConnectionConfiguration
    {
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        
        public string ResponseQueue { get; set; }
        
        public string ResponseRoutingKey { get; set; }
    }
}