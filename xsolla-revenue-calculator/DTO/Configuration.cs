namespace xsolla_revenue_calculator.DTO
{
    /// <summary>
    /// To use to extract data from appsettings.json
    /// </summary>
    public class Configuration
    {
        public MongoDbCredentials MongoDbCredentials { get; set; }
        
        public RabbitMQCredentials RabbitMQCredentials { get; set; }
    }
    
    
    public class MongoDbCredentials
    {
        public string Uri { get; set; }
    }

    public class RabbitMQCredentials
    {
        public string Host { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        
        public string ResponseQueue { get; set; }
        
        public string ResponseRoutingKeyBase { get; set; }
    }
}