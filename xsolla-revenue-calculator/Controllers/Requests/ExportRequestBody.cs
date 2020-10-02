using System.Text.Json.Serialization;

namespace xsolla_revenue_calculator.Controllers.Requests
{
    public class ExportRequestBody
    {
        [JsonPropertyName("revenueForecastId")]
        public string RevenueForecastId { get; set; }
        
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}