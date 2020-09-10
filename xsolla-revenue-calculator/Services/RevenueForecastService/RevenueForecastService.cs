using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Services.DatabaseAccessService;
using xsolla_revenue_calculator.Services.ModelMessagingService;

namespace xsolla_revenue_calculator.Services.RevenueForecastService
{
    public class RevenueForecastService : IRevenueForecastService
    {
        private readonly IDatabaseAccessService _databaseAccessService;
        private readonly IModelMessagingService _modelMessagingService;

        public RevenueForecastService(IDatabaseAccessService databaseAccessService, IModelMessagingService modelMessagingService)
        {
            _databaseAccessService = databaseAccessService;
            _modelMessagingService = modelMessagingService;
        }

        public async Task<RevenueForecast> StartCalculationAsync(UserInfo userInfo)
        {
            var draftForecast = await _databaseAccessService.PrepareForecastAsync();
            var messageToModel = PrepareMessageToModel(userInfo, draftForecast);
            await _modelMessagingService.SendAsync(messageToModel);
            return draftForecast;
        }

        private MessageToModel PrepareMessageToModel(UserInfo userInfo, RevenueForecast revenueForecast)
        {
            return new MessageToModel
            {
                RevenueForecastId = revenueForecast.Id.ToString(),
                Message = userInfo.Email
            };
        }
    }
}