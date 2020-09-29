using System.Text.Json.Serialization;

namespace xsolla_revenue_calculator.Controllers.Requests
{
    public class ExportRequestBody
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}