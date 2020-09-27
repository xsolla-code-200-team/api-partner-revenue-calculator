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
using xsolla_revenue_calculator.Exceptions;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Models.ForecastModels;
using xsolla_revenue_calculator.Models.UserInfoModels;

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

        public async Task<FullUserInfo> LogUserAsync(UserInfoFullRequestBody userInfoDto)
        {
            userInfoDto.ForecastType = ForecastType.Absolute;
            var userInfoDoc = userInfoDto.ToBsonDocument();
            await _users.InsertOneAsync(userInfoDoc);
            var userInfoModel = BsonSerializer.Deserialize<UserInfoFullRequestBody>(userInfoDoc);
            var userInfo = _mapper.Map<FullUserInfo>(userInfoModel);
            return userInfo;
        }
        
        public async Task<FullUserInfo> LogUserAsync(UserInfoBaseRequestBody userInfoDto)
        {
            userInfoDto.ForecastType = ForecastType.Percentage;
            var userInfoDoc = userInfoDto.ToBsonDocument();
            await _users.InsertOneAsync(userInfoDoc);
            var userInfoModel = BsonSerializer.Deserialize<UserInfoBaseRequestBody>(userInfoDoc);
            var userInfo = _mapper.Map<FullUserInfo>(userInfoModel);
            return userInfo;
        }

        public async Task AttachForecastToUserAsync(ObjectId userId, ObjectId forecastId)
        {
            var filter = new BsonDocument("_id", userId);
            var update = Builders<BsonDocument>.Update.Set("RevenueForecastId", forecastId);
            var result = await _users.UpdateOneAsync(filter, update);
        }

        private FullUserInfo GetUserInfoFromDocument(BsonDocument document)
        {
            var value = document.GetValue("ForecastType");
            return value.AsString == ForecastType.Absolute.ToString() ?
                _mapper.Map<FullUserInfo>(BsonSerializer.Deserialize<UserInfoFullRequestBody>(document)) : 
                _mapper.Map<FullUserInfo>(BsonSerializer.Deserialize<UserInfoBaseRequestBody>(document));
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
            var forecastsList = await (
                                        await _forecasts.FindAsync(forecast => 
                                            forecast.Id == new ObjectId(id)))
                                .ToListAsync();
            if (forecastsList.Count == 0) throw new ItemNotFoundException("RevenueForecasts");
            return forecastsList.Single();
        }

        public async Task<FullUserInfo> GetUserInfoByForecastId(string id)
        {
            var filter = new BsonDocument("RevenueForecastId", new ObjectId(id));
            var userInfoDocuments = await _users.FindAsync(filter);
            var userInfoList = await userInfoDocuments.ToListAsync();
            if (userInfoList.Count == 0) throw new ItemNotFoundException("UserInfo");
            var userInfoDocument = userInfoList.First();
            var userInfo = GetUserInfoFromDocument(userInfoDocument);
            return userInfo;
        }
    }

    public interface IDatabaseAccessService
    {
        Task<FullUserInfo> LogUserAsync(UserInfoFullRequestBody userInfo);
        
        Task<FullUserInfo> LogUserAsync(UserInfoBaseRequestBody userInfo);

        Task AttachForecastToUserAsync(ObjectId userId, ObjectId forecastId);

        Task<RevenueForecasts> CreateForecastAsync(ForecastType forecastType);
        Task<RevenueForecasts> GetForecastAsync(string id);
        Task<RevenueForecasts> UpdateForecastAsync(MessageFromModel message);
        Task<FullUserInfo> GetUserInfoByForecastId(string id);

    }
}