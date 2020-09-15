using System;
using MongoDB.Bson;

namespace xsolla_revenue_calculator.DTO
{
    /// <summary>
    /// To use for sending messages to model via RabbitMQ
    /// </summary>
    public class MessageToModel
    {
        public string RevenueForecastId { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return $"RevenueForecastID: {RevenueForecastId}, Message: {Message}";
        }
    }
}