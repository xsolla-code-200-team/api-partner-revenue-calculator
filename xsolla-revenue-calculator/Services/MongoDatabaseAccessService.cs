using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.DTO.Configuration;
using xsolla_revenue_calculator.DTO.MqMessages;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.Services
{
    public class MongoDatabaseAccessService : IDatabaseAccessService
    {
        private readonly IMongoDbConfiguration _mongoDbConfiguration;
        private MongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<RevenueForecasts> _forecasts;
        private IMongoCollection<BsonDocument> _users;
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
            _users = _database.GetCollection<BsonDocument>(_mongoDbConfiguration.UsersCollection);
        }

        public async Task<UserInfo> LogUserAsync(UserComplexFormDto userInfoDto)
        {
            var userInfoDoc = userInfoDto.ToBsonDocument();
            await _users.InsertOneAsync(userInfoDoc);
            var userInfoModel = BsonSerializer.Deserialize<UserComplexFormDto>(userInfoDoc);
            var userInfo = _mapper.Map<UserInfo>(userInfoModel);
            userInfo.ForecastType = ForecastType.Absolute;
            return userInfo;
        }
        
        public async Task<UserInfo> LogUserAsync(UserSimpleFormDto userInfoDto)
        {
            var userInfoDoc = userInfoDto.ToBsonDocument();
            await _users.InsertOneAsync(userInfoDoc);
            var userInfoModel = BsonSerializer.Deserialize<UserSimpleFormDto>(userInfoDoc);
            var userInfo = _mapper.Map<UserInfo>(userInfoModel);
            userInfo.ForecastType = ForecastType.Percentage;
            return userInfo;
        }

        public async Task AttachForecastToUserAsync(ObjectId userId, ObjectId forecastId)
        {
            var filter = new BsonDocument("_id", userId);
            var update = Builders<BsonDocument>.Update.Set("RevenueForecastId", forecastId);
            var result = await _users.UpdateOneAsync(filter, update);
        }

        private UserInfo GetUserInfoFromDocument(BsonDocument document)
        {
            var value = document.GetValue("ForecastType");
            return value.AsString == ForecastType.Absolute.ToString() ?
                _mapper.Map<UserInfo>(BsonSerializer.Deserialize<UserComplexFormDto>(document)) : 
                _mapper.Map<UserInfo>(BsonSerializer.Deserialize<UserSimpleFormDto>(document));
        }

        public async Task<RevenueForecasts> CreateForecastAsync(ForecastType forecastType)
        {
            var forecast = new RevenueForecasts
            {
                IsReady = false,
                ForecastType = forecastType
            };
            await _forecasts.InsertOneAsync(forecast);
            return forecast;
        }

        public async Task<RevenueForecasts> UpdateForecastAsync(MessageFromModel message)
        {
            var forecast = (await _forecasts.FindAsync(f => f.Id == new ObjectId(message.RevenueForecastId))).Single();
            forecast.ChosenForecast = message.ChosenForecast;
            forecast.OtherForecasts = message.OtherForecasts;
            forecast.IsReady = true;
            var result = await _forecasts.ReplaceOneAsync(f => f.Id == new ObjectId(message.RevenueForecastId), forecast);
            return forecast;
        }

        public async Task<RevenueForecasts> GetForecastAsync(string id)
        {
            return (await _forecasts.FindAsync(forecast => forecast.Id == new ObjectId(id))).Single();
        }

        public async Task<UserInfo> GetUserInfoByForecastId(string id)
        {
            var filter = new BsonDocument("RevenueForecastId", new ObjectId(id));
            var userInfoDocuments = await _users.FindAsync(filter);
            var userInfoDocument = userInfoDocuments.Single();
            var userInfo = GetUserInfoFromDocument(userInfoDocument);
            return userInfo;
        }
    }

    public interface IDatabaseAccessService
    {
        Task<UserInfo> LogUserAsync(UserComplexFormDto userInfo);
        
        Task<UserInfo> LogUserAsync(UserSimpleFormDto userInfo);

        Task AttachForecastToUserAsync(ObjectId userId, ObjectId forecastId);

        Task<RevenueForecasts> CreateForecastAsync(ForecastType forecastType);
        Task<RevenueForecasts> GetForecastAsync(string id);
        Task<RevenueForecasts> UpdateForecastAsync(MessageFromModel message);
        Task<UserInfo> GetUserInfoByForecastId(string id);

    }
}