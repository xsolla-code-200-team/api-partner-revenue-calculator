using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace xsolla_revenue_calculator.Models.ForecastModels
{
    public class RevenueForecasts : ForecastsInfo
    {
        public ObjectId Id { get; set; }
        public bool IsReady { get; set; }

        [BsonRepresentation(BsonType.String)] 
        public ForecastType ForecastType { get; set; }
    }
}