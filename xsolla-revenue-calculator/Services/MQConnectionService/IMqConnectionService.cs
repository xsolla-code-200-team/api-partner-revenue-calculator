using RabbitMQ.Client;

namespace xsolla_revenue_calculator.Services.MQConnectionService
{
    public interface IMqConnectionService
    {
        public IModel Channel { get; set; }
    }
}