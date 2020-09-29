using System;
using System.Threading.Tasks;
using xsolla_revenue_calculator.DTO.Configuration;
using xsolla_revenue_calculator.DTO.MqMessages.StaticAnalytics;
using xsolla_revenue_calculator.Services.CachingService;
using xsolla_revenue_calculator.Services.MessagingService;

namespace xsolla_revenue_calculator.Services
{
    public class StaticAnalyticsService : IStaticAnalyticsService
    {
        private readonly IRabbitMqConfiguration _rabbitMqConfiguration;
        private readonly IModelMessagingService _messagingService;

        public StaticAnalyticsService(IRabbitMqConfiguration rabbitMqConfiguration, IModelMessagingService messagingService)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration;
            _messagingService = messagingService;
        }

        public async Task RequestStaticAnalytics()
        {
            var request = new AnalyticsRequestToModel {Message = "sample message"};
            var config = _rabbitMqConfiguration.StaticInfoRpcConfiguration;
            _messagingService.ResponseProcessor = ProcessModelResponse;
            await _messagingService.SendAsync(config, request);
        }

        private async void ProcessModelResponse(IModelMessagingService sender, object message)
        {
            Console.WriteLine(message);
        }
    }

    public interface IStaticAnalyticsService
    {
        Task RequestStaticAnalytics();
    }
}