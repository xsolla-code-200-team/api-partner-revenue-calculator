using System;
using System.Threading.Tasks;
using AutoMapper;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.DTO.MqMessages;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Models.ForecastModels;
using xsolla_revenue_calculator.Models.UserInfoModels;
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

        public async Task<RevenueForecasts> StartCalculationAsync(FullUserInfo fullUserInfo)
        {
            var existingForecast = await _cachingService.GetRevenueForecastAsync(fullUserInfo);
            if (existingForecast != null) return existingForecast;
            var draftForecast = await _databaseAccessService.CreateForecastAsync(fullUserInfo.ForecastType);
            var messageToModel = PrepareMessageToModel(fullUserInfo, draftForecast);
            _modelMessagingService.ResponseProcessor = ModelResponseProcessor;
            await _modelMessagingService.SendAsync(messageToModel);
            return draftForecast;
        }
        

        private async void ModelResponseProcessor(IModelMessagingService sender, MessageFromModel message)
        {
            var forecast = await _databaseAccessService.UpdateForecastAsync(message);
            await _cachingService.AddForecastToCacheAsync(forecast);
            Console.WriteLine(message);
            sender.Dispose();
        }
        
        private MessageToModel PrepareMessageToModel(FullUserInfo fullUserInfo, RevenueForecasts forecast)
        {
            var messageToModel = _mapper.Map<MessageToModel>(fullUserInfo);
            messageToModel.ForecastType = forecast.ForecastType.ToString();
            messageToModel.RevenueForecastId = forecast.Id.ToString();
            return messageToModel;
        }
    }
    
    public interface IRevenueForecastService
    {
        Task<RevenueForecasts> StartCalculationAsync(FullUserInfo fullUserInfo);
    }
}