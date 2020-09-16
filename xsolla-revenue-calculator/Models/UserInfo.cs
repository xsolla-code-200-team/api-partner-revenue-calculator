using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace xsolla_revenue_calculator.Models
{
    public class UserInfo
    {
        public ObjectId Id { get; set; }
        
        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }
        
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
        
        [JsonPropertyName("sales")]
        public string Sales { get; set; }
        
        [JsonPropertyName("score")]
        public string Score { get; set; }
        
        [JsonPropertyName("email")]
        [EmailAddress]
        public string Email { get; set; }
        
    }
}