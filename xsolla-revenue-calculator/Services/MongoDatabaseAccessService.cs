using System;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.DTO.Configuration;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.Services
{
    public class MongoDatabaseAccessService : IDatabaseAccessService
    {
        private readonly IMongoDbConfiguration _mongoDbConfiguration;
        private MongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<RevenueForecasts> _forecasts;
        private IMongoCollection<UserInfo> _users;
        private readonly IMapper _mapper;

        public MongoDatabaseAccessService(IMongoDbConfiguration mongoDbConfiguration, IMapper mapper)
        {
            _mongoDbConfiguration = mongoDbConfiguration;
            _mapper = mapper;
            ConfigureService();
        }

        private void ConfigureService()
        {
            var connectionString = _mongoDbConfiguration.Uri;
            var password = Environment.GetEnvironmentVariable("MONGODB_PASSWORD") ??
                           _mongoDbConfiguration.DefaultPassword;
            connectionString = connectionString.Replace("<password>", password);
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("main");
            _forecasts = _database.GetCollection<RevenueForecasts>(_mongoDbConfiguration.ForecastsCollection);
            _users = _database.GetCollection<UserInfo>(_mongoDbConfiguration.UsersCollection);
        }

        public async Task<UserInfo> LogUserAsync(UserComplexFormDto userInfoDto)
        {
            var userInfo = _mapper.Map<UserInfo>(userInfoDto);
            userInfo.ForecastType = ForecastType.Absolute;
            await _users.InsertOneAsync(userInfo);
            return userInfo;
        }
        
        public async Task<UserInfo> LogUserAsync(UserSimpleFormDto userInfoDto)
        {
            var userInfo = _mapper.Map<UserInfo>(userInfoDto);
            userInfo.ForecastType = ForecastType.Percentage;
            await _users.InsertOneAsync(userInfo);
            return userInfo;
        }
        
        public async Task<RevenueForecasts> CreateForecastAsync()
        {
            var forecast = new RevenueForecasts
            {
                IsReady = false
            };
            await _forecasts.InsertOneAsync(forecast);
            return forecast;
        }

        public async Task UpdateForecastAsync(MessageFromModel message)
        {
            var forecast = (await _forecasts.FindAsync(f => f.Id == new ObjectId(message.RevenueForecastId))).Single();
            forecast.ChosenForecast = message.ChosenForecast;
            forecast.OtherForecasts = message.OtherForecasts;
            var result = await _forecasts.ReplaceOneAsync(f => f.Id == new ObjectId(message.RevenueForecastId), forecast);
        }

        public async Task<RevenueForecasts> GetForecastAsync(string id)
        {
            return (await _forecasts.FindAsync(forecast => forecast.Id == new ObjectId(id))).Single();
        }
    }

    public interface IDatabaseAccessService
    {
        Task<UserInfo> LogUserAsync(UserComplexFormDto userInfo);
        
        Task<UserInfo> LogUserAsync(UserSimpleFormDto userInfo);

        Task<RevenueForecasts> CreateForecastAsync();
        Task<RevenueForecasts> GetForecastAsync(string id);
        Task UpdateForecastAsync(MessageFromModel message);

    }
}