namespace xsolla_revenue_calculator.DTO
{
    public class Configuration
    {
        public MongoDbCredentials MongoDbCredentials { get; set; }
    }
    
    
    public class MongoDbCredentials
    {
        public string Uri { get; set; }
    }
}