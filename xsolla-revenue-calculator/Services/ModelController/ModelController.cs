using System;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using xsolla_revenue_calculator.DTO;

namespace xsolla_revenue_calculator.Services.ModelController
{
    public class ModelController : IModelController
    {
        private readonly IOptions<Configuration> _configuration;

        private string _exchangeName;
        private string _hostName;
        private string _routingKey;

        private IModel _channel;

        public ModelController(IOptions<Configuration> configuration)
        {
            _configuration = configuration;
            ConfigureMessageProducer();
            InitializeExchange();
        }

        private void ConfigureMessageProducer()
        {
            _exchangeName = _configuration.Value.RabbitMQCredentials.Exchange;
            _hostName = Environment.GetEnvironmentVariable("CLOUDAMQP_URL") ?? _configuration.Value.RabbitMQCredentials.Host;
            _routingKey = _configuration.Value.RabbitMQCredentials.RoutingKey;
        }
        
        private void InitializeExchange()
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
            
            _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct);
        }
        
        public void Publish(string message)
        {
            var messageModel = GetMessage(message);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageModel));
            _channel.BasicPublish(_exchangeName, _routingKey, null, body);
            Console.WriteLine(" [x] Sent {0}", message);
        }

        private static MessageToModel GetMessage(string payload)
        {
            var message = new MessageToModel { Message= payload};
            return message;
        }
        
    }
}