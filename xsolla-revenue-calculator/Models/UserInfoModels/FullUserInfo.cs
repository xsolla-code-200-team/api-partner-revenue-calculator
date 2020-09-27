using System.Text.Json.Serialization;

namespace xsolla_revenue_calculator.Models.UserInfoModels
{
    public class FullUserInfo : BaseUserInfo
    {

        [JsonPropertyName("releaseDate")]
        public string ReleaseDate { get; set; }
        
        [JsonPropertyName("sales")]
        public string Sales { get; set; }
        
        [JsonPropertyName("cost")]
        public string Cost { get; set; }

    }
}