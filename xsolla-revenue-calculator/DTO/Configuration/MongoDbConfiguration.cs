namespace xsolla_revenue_calculator.DTO.Configuration
{
    public class MongoDbConfiguration : IMongoDbConfiguration
    {
        public string Uri { get; set; }
        public string DefaultPassword { get; set; }
        public string Database { get; set; }
        public string UsersCollection { get; set; }
        public string ForecastsCollection { get; set; }

    }

    public interface IMongoDbConfiguration
    {
        public string Uri { get; set; }
        public string DefaultPassword { get; set; }
        public string Database { get; set; }
        public string UsersCollection { get; set; }
        public string ForecastsCollection { get; set; }
    }
}