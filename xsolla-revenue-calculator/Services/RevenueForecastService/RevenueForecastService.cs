using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public RevenueForecastService(IDatabaseAccessService databaseAccessService,
            IModelMessagingService modelMessagingService, IMapper mapper)
        {
            _databaseAccessService = databaseAccessService;
            _modelMessagingService = modelMessagingService;
            _mapper = mapper;
        }

        public async Task<RevenueForecast> StartCalculationAsync(UserInfo userInfo)
        {
            var draftForecast = await _databaseAccessService.PrepareForecastAsync();
            var messageToModel = PrepareMessageToModel(userInfo, draftForecast);
            _modelMessagingService.ResponseProcessor = ResponseProcessor;
            await _modelMessagingService.SendAsync(messageToModel);
            return draftForecast;
        }

        private void ResponseProcessor(IModelMessagingService sender, MessageFromModel message)
        {
            _databaseAccessService.UpdateRevenueForecast(message);
            Console.WriteLine(message);
            sender.Dispose();
        }

        private MessageToModel PrepareMessageToModel(UserInfo userInfo, RevenueForecast revenueForecast)
        {
            var message = _mapper.Map<MessageToModel>(userInfo);
            message.RevenueForecastId = revenueForecast.Id.ToString();
            return message;
        }
    }
}