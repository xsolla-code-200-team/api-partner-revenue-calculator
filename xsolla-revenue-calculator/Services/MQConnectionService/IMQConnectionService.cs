using RabbitMQ.Client;

namespace xsolla_revenue_calculator.Services.MQConnectionService
{
    public interface IMQConnectionService
    {
        public IModel Channel { get; set; }
    }
}