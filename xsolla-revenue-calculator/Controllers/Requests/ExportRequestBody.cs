using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace xsolla_revenue_calculator.Controllers.Requests
{
    public class ExportRequestBody
    {
        [JsonPropertyName("revenueForecastId")]
        public string RevenueForecastId { get; set; }
        
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        
        [JsonPropertyName("revenue")]
        public string Revenue { get; set; }
        
        [JsonPropertyName("monetization")]
        public string Monetization { get; set; }
            
        [JsonPropertyName("topMarket")]
        public string TopMarket { get; set; }
    }
}