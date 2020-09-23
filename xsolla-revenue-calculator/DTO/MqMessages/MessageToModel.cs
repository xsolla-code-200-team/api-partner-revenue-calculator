using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.DTO
{
    /// <summary>
    /// To use for sending messages to model via RabbitMQ
    /// </summary>
    public class MessageToModel
    {
        [JsonPropertyName("revenueForecastID")]
        public string RevenueForecastId { get; set; }
        
        [JsonPropertyName("forecastType")]
        public string ForecastType { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; }
        
        [JsonPropertyName("releaseDate")]
        public string ReleaseDate { get; set; }
        
        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }
        
        [JsonPropertyName("monetization")]
        public string Monetization { get; set; }
        
        [JsonPropertyName("platforms")]
        public List<string> Platforms { get; set; }
        
        [JsonPropertyName("regions")]
        public List<string> Regions { get; set; }
        
        [JsonPropertyName("sales")]
        public string Sales { get; set; }
        
        [JsonPropertyName("cost")]
        public string Cost { get; set; }
        
        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }
        
        [JsonPropertyName("email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}