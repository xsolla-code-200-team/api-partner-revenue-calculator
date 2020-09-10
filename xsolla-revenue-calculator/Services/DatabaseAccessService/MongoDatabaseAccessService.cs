using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.Services.DatabaseAccessService
{
    public class MongoDatabaseAccessService : IDatabaseAccessService
    {
        private readonly IOptions<Configuration> _configuration;
        private readonly IMapper _mapper;
        private MongoClient _client;
        private IMongoDatabase Database => _client.GetDatabase("main");

        public MongoDatabaseAccessService(IOptions<Configuration> configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            ConfigureClient();
        }
        
        private void ConfigureClient()
        {
            var connectionString = _configuration.Value.MongoDbCredentials.Uri;
            var password = Environment.GetEnvironmentVariable("MONGODB_PASSWORD") ?? "dbPassword";
            connectionString = connectionString.Replace("<password>", password);
            _client = new MongoClient(connectionString);
        }

        public async Task<UserInfo> LogUserAsync(UserInfo userInfo)
        {
            var collection = Database.GetCollection<UserInfo>("users");
            await collection.InsertOneAsync(userInfo);
            return userInfo;
        }

        public async Task<RevenueForecast> PrepareForecastAsync()
        {
            var collection = Database.GetCollection<RevenueForecast>("revenue-forecasts");
            var forecast = new RevenueForecast
            {
                IsReady = false
            };
            await collection.InsertOneAsync(forecast);
            return forecast;
        }
    }
}