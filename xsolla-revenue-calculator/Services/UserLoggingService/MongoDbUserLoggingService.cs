using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.Services.UserLoggingService
{
    public class MongoDbUserLoggingService : IUserLoggingService
    {
        private readonly IOptions<Configuration> _configuration;
        private readonly IMapper _mapper;
        private MongoClient _client;
        private IMongoDatabase _database => _client.GetDatabase("main");

        public MongoDbUserLoggingService(IOptions<Configuration> configuration, IMapper mapper)
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

        public async Task<UserInfo> LogUser(UserInfoRequestBody userInfoRequestBody)
        {
            var collection = _database.GetCollection<UserInfo>("users");
            var userInfo = _mapper.Map<UserInfo>(userInfoRequestBody);
            await collection.InsertOneAsync(userInfo);
            return userInfo;
        }
    }
}