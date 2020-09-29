using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using xsolla_revenue_calculator.DTO.Configuration;
using xsolla_revenue_calculator.DTO.MqMessages;
using xsolla_revenue_calculator.DTO.MqMessages.StaticAnalytics;
using xsolla_revenue_calculator.Exceptions;
using xsolla_revenue_calculator.Models.StaticAnalyticsModels;
using xsolla_revenue_calculator.Services.CachingService;
using xsolla_revenue_calculator.Services.MessagingService;

namespace xsolla_revenue_calculator.Services
{
    public class StaticAnalyticsService : IStaticAnalyticsService
    {
        private readonly IRabbitMqConfiguration _rabbitMqConfiguration;
        private readonly IModelMessagingService _messagingService;
        private readonly IRedisAccessService _redisAccessService;

        public StaticAnalyticsService(IRabbitMqConfiguration rabbitMqConfiguration, IModelMessagingService messagingService, IRedisAccessService redisAccessService)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration;
            _messagingService = messagingService;
            _redisAccessService = redisAccessService;
        }

        public async Task RequestStaticAnalytics()
        {
            var request = new AnalyticsRequestToModel {Message = "sample message"};
            var config = _rabbitMqConfiguration.StaticInfoRpcConfiguration;
            _messagingService.ResponseProcessor = ProcessModelResponse;
            await _messagingService.SendAsync(config, request);
        }

        public async Task<GenreInfo> GetGenreInfo(string genre)
        {
            var genreInfo = new GenreInfo
            {
                Genre = genre
            };
            var info = await _redisAccessService.GetAsync(genre);
            if (info == null) throw new ItemNotFoundException("GenreInfo");
            var regionsInfo = JsonConvert.DeserializeObject<List<RegionInfo>>(info);
            genreInfo.RegionsInfo = regionsInfo;
            return genreInfo;
        } 

        private async void ProcessModelResponse(IModelMessagingService sender, object message)
        {
            ITraceWriter traceWriter = new MemoryTraceWriter();
            var analyticsFromModel = JsonConvert.DeserializeObject<StaticInfo>((string)message, new JsonSerializerSettings{TraceWriter = traceWriter});
            foreach (GenreInfo genreInfo in analyticsFromModel.GenresInfo)
            {
                var regionsInfo = JsonConvert.SerializeObject(genreInfo.RegionsInfo);
                await _redisAccessService.SetAsync(genreInfo.Genre, regionsInfo);
            }
        }
    }

    public interface IStaticAnalyticsService
    {
        Task RequestStaticAnalytics();
        Task<GenreInfo> GetGenreInfo(string genre);

    }
}