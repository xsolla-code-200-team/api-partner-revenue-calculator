using System;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using xsolla_revenue_calculator.DTO;

namespace xsolla_revenue_calculator.Services.MQConnectionService
{
    public class MQConnectionService : IDisposable, IMQConnectionService
    {
        private string _hostName;
        private readonly IOptions<Configuration> _configuration;
        private IConnection _connection;
        public IModel Channel { get; set; }

        public MQConnectionService(IOptions<Configuration> configuration)
        {
            _configuration = configuration;
            ConfigureParameters();
            InitializeChannel();
        }

        private void ConfigureParameters()
        {
            var credentials = _configuration.Value.RabbitMQCredentials;
            _hostName = Environment.GetEnvironmentVariable("CLOUDAMQP_URL") ?? credentials.Host;
        }

        private void InitializeChannel()
        {
            if (_hostName == _configuration.Value.RabbitMQCredentials.Host)
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
}