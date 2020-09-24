using System;
using RabbitMQ.Client;
using xsolla_revenue_calculator.DTO.Configuration;

namespace xsolla_revenue_calculator.Services.MessagingService
{
    public class MqConnectionService : IDisposable, IMqConnectionService
    {
        private string _hostName;
        private readonly IRabbitMqConfiguration _rabbitMqConfiguration;
        private IConnection _connection;
        public IModel Channel { get; set; }

        public MqConnectionService(IRabbitMqConfiguration rabbitMqConfiguration)
        {
            _rabbitMqConfiguration = rabbitMqConfiguration;
            ConfigureParameters();
            InitializeChannel();
        }

        private void ConfigureParameters()
        {
            _hostName = Environment.GetEnvironmentVariable("CLOUDAMQP_URL") ?? _rabbitMqConfiguration.Host;
        }

        private void InitializeChannel()
        {
            if (_hostName == _rabbitMqConfiguration.Host)
                _connection = new ConnectionFactory
                {
                    HostName = _hostName
                }.CreateConnection();
            else
                _connection = new ConnectionFactory
                {
                    Uri = new Uri(_hostName)
                }.CreateConnection();
            Channel = _connection.CreateModel();

        }

        public void Dispose()
        {
            Channel.Dispose();
            _connection.Dispose();
        }
    }
    public interface IMqConnectionService
    {
        public IModel Channel { get; set; }
    }
}