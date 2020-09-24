using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace xsolla_revenue_calculator.DTO.MqMessages
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
    }
}