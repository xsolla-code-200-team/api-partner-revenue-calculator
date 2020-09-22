namespace xsolla_revenue_calculator.DTO.Configuration
{
    public class RabbitMqConfiguration : IRabbitMqConfiguration
    {
        public string Host { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string ResponseQueue { get; set; }
        public string ResponseRoutingKeyBase { get; set; }
    }

    public interface IRabbitMqConfiguration
    {
        public string Host { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string ResponseQueue { get; set; }
        public string ResponseRoutingKeyBase { get; set; }
    }
}