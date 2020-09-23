using System;
using System.Threading.Tasks;
using AutoMapper;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.Services
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

        public async Task<RevenueForecasts> StartCalculationAsync(UserInfo userInfo)
        {
            var draftForecast = await _databaseAccessService.CreateForecastAsync(userInfo);
            var messageToModel = PrepareMessageToModel(userInfo, draftForecast);
            _modelMessagingService.ResponseProcessor = ModelResponseProcessor;
            await _modelMessagingService.SendAsync(messageToModel);
            return draftForecast;
        }

        private void ModelResponseProcessor(IModelMessagingService sender, MessageFromModel message)
        {
            _databaseAccessService.UpdateForecastAsync(message);
            Console.WriteLine(message);
            sender.Dispose();
        }

        private MessageToModel PrepareMessageToModel(UserInfo userInfo, RevenueForecasts revenueForecasts)
        {
            var message = _mapper.Map<MessageToModel>(userInfo);
            message.RevenueForecastId = revenueForecasts.Id.ToString();
            message.ForecastType = revenueForecasts.ForecastType.ToString();
            return message;
        }
    }
    
    public interface IRevenueForecastService
    {
        Task<RevenueForecasts> StartCalculationAsync(UserInfo userInfo);
    }
}