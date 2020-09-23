using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.DTO.Configuration;

namespace xsolla_revenue_calculator.Services 
{
    public class ModelMessagingService : IModelMessagingService
    {
        private readonly IRabbitMqConfiguration _rabbitMqConfiguration;
        private readonly IMqConnectionService _connectionService;

        private string _exchangeName;
        private string _routingKey;
        private string _responseQueue;
        private string _responseRoutingKey;
        public Action<IModelMessagingService, MessageFromModel> ResponseProcessor { get; set; }

        public ModelMessagingService(IRabbitMqConfiguration rabbitMqConfiguration, IMqConnectionService connectionService)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration;
            _connectionService = connectionService;
            ConfigureParameters();
            InitializeExchange();
        }
        
        private void ConfigureParameters()
        {
            _exchangeName = _rabbitMqConfiguration.Exchange;
            _routingKey = _rabbitMqConfiguration.RoutingKey;
            _responseQueue = _rabbitMqConfiguration.ResponseQueue;
            _responseRoutingKey = _rabbitMqConfiguration.ResponseRoutingKeyBase;
        }
        private void InitializeExchange()
        {
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
        
        public async Task SendAsync(MessageToModel message)
        {
            _responseRoutingKey += $"-{message.RevenueForecastId}";
            InitializeResponseQueue();
            await Task.Run(
                () =>
                {
                    Console.WriteLine(JsonConvert.SerializeObject(message));
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                    _connectionService.Channel.BasicPublish(_exchangeName, _routingKey, null, body);
                    Console.WriteLine(" [x] Sent {0}", message.RevenueForecastId);
                }
            );
        }

        private MessageFromModel GetMessageFromModel(BasicDeliverEventArgs args)
        {
            var messageString = Encoding.UTF8.GetString(args.Body.ToArray());
            return JsonConvert.DeserializeObject<MessageFromModel>(messageString);
        }
        
        public void Dispose()
        {
            DisposeResponseQueue();
        }
    }
    
    public interface IModelMessagingService
    {
        Task SendAsync(MessageToModel message);
        Action<IModelMessagingService, MessageFromModel> ResponseProcessor { get; set; }

        void Dispose();
    }
}