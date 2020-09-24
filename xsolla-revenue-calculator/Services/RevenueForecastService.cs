using System;
using System.Threading.Tasks;
using AutoMapper;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.DTO.MqMessages;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Services.CachingService;
using xsolla_revenue_calculator.Services.MessagingService;

namespace xsolla_revenue_calculator.Services
{
    public class RevenueForecastService : IRevenueForecastService
    {
        private readonly IDatabaseAccessService _databaseAccessService;
        private readonly IModelMessagingService _modelMessagingService;
        private readonly IForecastCachingService _cachingService;
        private readonly IMapper _mapper;

        public RevenueForecastService(IDatabaseAccessService databaseAccessService,
            IModelMessagingService modelMessagingService, IMapper mapper, IForecastCachingService cachingService)
        {
            _databaseAccessService = databaseAccessService;
            _modelMessagingService = modelMessagingService;
            _mapper = mapper;
            _cachingService = cachingService;
        }

        public async Task<RevenueForecasts> StartCalculationAsync(UserInfo userInfo)
        {
            //var existingForecastId = await _cachingService.GetRevenueForecastIdAsync(userInfo);
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
        
        private MessageToModel PrepareMessageToModel(UserInfo userInfo, RevenueForecasts forecast)
        {
            var messageToModel = _mapper.Map<MessageToModel>(userInfo);
            messageToModel.ForecastType = forecast.ForecastType.ToString();
            messageToModel.RevenueForecastId = forecast.Id.ToString();
            return messageToModel;
        }
    }
    
    public interface IRevenueForecastService
    {
        Task<RevenueForecasts> StartCalculationAsync(UserInfo userInfo);
    }
}