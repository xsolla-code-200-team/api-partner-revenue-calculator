using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Services.MQConnectionService;

namespace xsolla_revenue_calculator.Services.ModelMessagingService 
{
    public class ModelMessagingService : IModelMessagingService
    {
        private readonly IOptions<Configuration> _configuration;
        private readonly IMQConnectionService _connectionService;

        private string _exchangeName;
        private string _routingKey;
        private string _responseQueue;
        private string _responseRoutingKey;
        public Action<IModelMessagingService, MessageFromModel> ResponseProcessor { get; set; }

        public ModelMessagingService(IOptions<Configuration> configuration, IMQConnectionService connectionService)
        {
            _configuration = configuration;
            _connectionService = connectionService;
            ConfigureParameters();
            InitializeExchange();
        }
        
        private void ConfigureParameters()
        {
            var credentials = _configuration.Value.RabbitMQCredentials;
            _exchangeName = credentials.Exchange;
            _routingKey = credentials.RoutingKey;
            _responseQueue = credentials.ResponseQueue;
            _responseRoutingKey = credentials.ResponseRoutingKeyBase;
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
}