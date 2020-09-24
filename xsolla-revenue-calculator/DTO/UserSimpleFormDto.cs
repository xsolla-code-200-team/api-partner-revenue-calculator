using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.DTO
{
    public class UserSimpleFormDto
    {
        public ObjectId Id { get; set; }
        
        [BsonRepresentation(BsonType.String)]
        public ForecastType ForecastType { get; set; } = ForecastType.Absolute;

        [JsonPropertyName("productName")]
        public string ProductName { get; set; }

        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }
        
        [JsonPropertyName("monetization")]
        public string Monetization { get; set; }
        
        [JsonPropertyName("platforms")]
        public List<string> Platforms { get; set; }
        
        [JsonPropertyName("regions")]
        public List<string> Regions { get; set; }

        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }
        
        [JsonPropertyName("email")]
        [EmailAddress]
        public string Email { get; set; }
        
        public ObjectId RevenueForecastId { get; set; }
    }
}