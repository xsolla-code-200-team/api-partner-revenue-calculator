using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json.Converters;

namespace xsolla_revenue_calculator.Models
{
    public class RevenueForecasts
    {
        public ObjectId Id { get; set; }
        public bool IsReady { get; set; }
        
        public ForecastType ForecastType { get; set; }
        
        public ParticularForecast ChosenForecast { get; set; }
        
        public List<ParticularForecast> OtherForecasts { get; set; } }
}