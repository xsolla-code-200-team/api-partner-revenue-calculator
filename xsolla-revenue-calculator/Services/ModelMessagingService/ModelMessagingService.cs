using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using xsolla_revenue_calculator.DTO;

namespace xsolla_revenue_calculator.Services.ModelMessagingService 
{
    public class ModelMessagingService : IModelMessagingService
    {
        private readonly IOptions<Configuration> _configuration;

        private string _exchangeName;
        private string _hostName;
        private string _routingKey;
        private string _responseQueue;
        private string _responseRoutingKey;
        private IModel _channel;
        public Action<IModelMessagingService, MessageFromModel> ResponseProcessor { get; set; }

        public ModelMessagingService(IOptions<Configuration> configuration)
        {
            _configuration = configuration;
            ConfigureMessageProducer();
            InitializeChannel();
            InitializeExchange();
        }

        private void ConfigureMessageProducer()
        {
            var credentials = _configuration.Value.RabbitMQCredentials;
            _exchangeName = credentials.Exchange;
            _hostName = Environment.GetEnvironmentVariable("CLOUDAMQP_URL") ?? credentials.Host;
            _routingKey = credentials.RoutingKey;
            _responseQueue = credentials.ResponseQueue;
            _responseRoutingKey = credentials.ResponseRoutingKeyBase;
        }

        private void InitializeChannel()
        {
            if (_hostName == _configuration.Value.RabbitMQCredentials.Host)
                _channel = new ConnectionFactory
                {
                    HostName = _hostName
                }.CreateConnection().CreateModel();
            else
                _channel = new ConnectionFactory
                {
                    Uri = new Uri(_hostName)
                }.CreateConnection().CreateModel();

        }
        private void InitializeExchange()
        {
            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
        }

        private void InitializeResponseQueue()
        {
            _channel.QueueDeclare(_responseQueue, exclusive: false);
            _channel.QueueBind(_responseQueue, _exchangeName, _responseRoutingKey);
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (_, args) =>
            {
                var message = GetMessageFromModel(args);
                ResponseProcessor?.Invoke(this, message);
            };    
            _channel.BasicConsume(_responseQueue, true, consumer);        
        }

        private void DisposeResponseQueue()
        {
            _channel.QueueUnbind(_responseQueue, _exchangeName, _responseRoutingKey);
            _channel.QueueDelete(_responseQueue);
        }
        
        public async Task SendAsync(MessageToModel message)
        {
            _responseRoutingKey += $"-{message.RevenueForecastId}";
            InitializeResponseQueue();
            await Task.Run(
                () =>
                {
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                    _channel.BasicPublish(_exchangeName, _routingKey, null, body);
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
            _channel?.Dispose();
        }
    }
}