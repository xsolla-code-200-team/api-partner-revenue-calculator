using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using xsolla_revenue_calculator.DTO.Configuration;
using xsolla_revenue_calculator.DTO.MqMessages;

namespace xsolla_revenue_calculator.Services.MessagingService 
{
    public class ModelMessagingService : IModelMessagingService
    {
        private readonly IRabbitMqConfiguration _rabbitMqConfiguration;
        private readonly IMqConnectionService _connectionService;

        private string _exchangeName;
        private string _routingKey;
        private string _responseQueue;
        private string _responseRoutingKey;
        public Action<IModelMessagingService, object> ResponseProcessor { get; set; }

        public ModelMessagingService(IRabbitMqConfiguration rabbitMqConfiguration, IMqConnectionService connectionService)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration;
            _connectionService = connectionService;
            InitializeExchange();
        }
        
        private void ConfigureParameters(RpcConnectionConfiguration configuration)
        {
            _routingKey = configuration.RoutingKey;
            _responseQueue = configuration.ResponseQueue;
            _responseRoutingKey = configuration.ResponseRoutingKey;
        }
        private void InitializeExchange()
        {
            _exchangeName = _rabbitMqConfiguration.Exchange;
            _connectionService.Channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
        }

        private void InitializeResponseQueue()
        {
            _connectionService.Channel.QueueDeclare(_responseQueue, exclusive: false);
            _connectionService.Channel.QueueBind(_responseQueue, _exchangeName, _responseRoutingKey);
            EventingBasicConsumer consumer = new EventingBasicConsumer(_connectionService.Channel);
            consumer.Received += (_, args) =>
            {
                var message = GetMessageFromModel(args);
                ResponseProcessor?.Invoke(this, message);
            };    
            _connectionService.Channel.BasicConsume(_responseQueue, true, consumer);        
        }

        private void DisposeResponseQueue()
        {
            _connectionService.Channel.QueueUnbind(_responseQueue, _exchangeName, _responseRoutingKey);
            _connectionService.Channel.QueueDelete(_responseQueue);
        }
        
        public async Task SendAsync(RpcConnectionConfiguration configuration, object message)
        {
            ConfigureParameters(configuration);
            if (message is UserInfoToModel model)
                _responseRoutingKey += $"-{model.RevenueForecastId}";
            InitializeResponseQueue();
            await Task.Run(
                () =>
                {
                    Console.WriteLine(JsonConvert.SerializeObject(message));
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                    _connectionService.Channel.BasicPublish(_exchangeName, _routingKey, null, body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            );
        }

        private ForecastFromModel GetMessageFromModel(BasicDeliverEventArgs args)
        {
            var messageString = Encoding.UTF8.GetString(args.Body.ToArray());
            return JsonConvert.DeserializeObject<ForecastFromModel>(messageString);
        }
        
        public void Dispose()
        {
            DisposeResponseQueue();
        }
    }
    
    public interface IModelMessagingService
    {
        Task SendAsync(RpcConnectionConfiguration configuration, object message);
        Action<IModelMessagingService, object> ResponseProcessor { get; set; }

        void Dispose();
    }
}