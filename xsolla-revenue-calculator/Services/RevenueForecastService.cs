using System;
using System.Threading.Tasks;
using AutoMapper;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Services.DatabaseAccessService;

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

        public async Task<RevenueForecast> StartCalculationAsync(UserInfo userInfo)
        {
            var draftForecast = await _databaseAccessService.CreateForecastAsync();
            var messageToModel = PrepareMessageToModel(userInfo, draftForecast);
            _modelMessagingService.ResponseProcessor = ResponseProcessor;
            await _modelMessagingService.SendAsync(messageToModel);
            return draftForecast;
        }

        private void ResponseProcessor(IModelMessagingService sender, MessageFromModel message)
        {
            _databaseAccessService.UpdateForecastAsync(message);
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
    
    public interface IRevenueForecastService
    {
        Task<RevenueForecast> StartCalculationAsync(UserInfo userInfo);
    }
}