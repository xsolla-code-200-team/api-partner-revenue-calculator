using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.DTO.Configuration;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.Services.DatabaseAccessService
{
    public class MongoDatabaseAccessService : IDatabaseAccessService
    {
        private readonly IMongoDbConfiguration _mongoDbConfiguration;
        private MongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<RevenueForecast> _forecasts;
        private IMongoCollection<UserInfo> _users;

        public MongoDatabaseAccessService(IMongoDbConfiguration mongoDbConfiguration)
        {
            _mongoDbConfiguration = mongoDbConfiguration;
            ConfigureService();
        }
        
        private void ConfigureService()
        {
            var connectionString = _mongoDbConfiguration.Uri;
            var password = Environment.GetEnvironmentVariable("MONGODB_PASSWORD") ?? _mongoDbConfiguration.DefaultPassword;
            connectionString = connectionString.Replace("<password>", password);
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("main");
            _forecasts = _database.GetCollection<RevenueForecast>(_mongoDbConfiguration.ForecastsCollection);
            _users = _database.GetCollection<UserInfo>(_mongoDbConfiguration.UsersCollection);
        }

        public async Task<UserInfo> LogUserAsync(UserInfo userInfo)
        {
            await _users.InsertOneAsync(userInfo);
            return userInfo;
        }

        public async Task<RevenueForecast> CreateForecastAsync()
        {
            var forecast = new RevenueForecast
            {
                IsReady = false
            };
            await _forecasts.InsertOneAsync(forecast);
            return forecast;
        }

        public async Task UpdateForecastAsync(MessageFromModel message)
        {
            var forecast = (await _forecasts.FindAsync(f => f.Id == new ObjectId(message.RevenueForecastId))).Single();
            forecast.RevenuePerMonth = message.Result;
            await _forecasts.ReplaceOneAsync(f => f.Id == new ObjectId(message.RevenueForecastId), forecast);
        }

        public async Task<RevenueForecast> GetForecastAsync(string id)
        {
            return (await _forecasts.FindAsync(forecast => forecast.Id == new ObjectId(id))).Single();
        }
    }
    
    public interface IDatabaseAccessService
    {
        Task<UserInfo> LogUserAsync(UserInfo userInfo);
        Task<RevenueForecast> CreateForecastAsync();
        Task<RevenueForecast> GetForecastAsync(string id);
        Task UpdateForecastAsync(MessageFromModel message);

    }
}