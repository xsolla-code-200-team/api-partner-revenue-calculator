using System;
using MongoDB.Bson;

namespace xsolla_revenue_calculator.DTO
{
    /// <summary>
    /// To use for sending via RabbitMQ
    /// </summary>
    public class MessageToModel
    {
        public ObjectId RevenueForecastId { get; set; }
        public string Message { get; set; }
    }
}