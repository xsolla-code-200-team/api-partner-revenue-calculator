using System.Text.Json.Serialization;

namespace xsolla_revenue_calculator.Models.UserInfoModels
{
    public class FullUserInfo : BaseUserInfo
    {

        [JsonPropertyName("releaseDate")]
        public string ReleaseDate { get; set; }
        
        [JsonPropertyName("sales")]
        public double Sales { get; set; }
        
        [JsonPropertyName("cost")]
        public double Cost { get; set; }
        
        [JsonPropertyName("initialRevenue")]
        public double InitialRevenue { get; set; }
        
        [JsonPropertyName("isReleased")]
        public bool IsReleased { get; set; }
    }
}