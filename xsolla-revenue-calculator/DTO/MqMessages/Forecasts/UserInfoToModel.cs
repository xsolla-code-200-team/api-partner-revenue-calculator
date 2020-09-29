using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Models.UserInfoModels;

namespace xsolla_revenue_calculator.DTO.MqMessages
{
    /// <summary>
    /// To use for sending messages to model via RabbitMQ
    /// </summary>
    public class UserInfoToModel : FullUserInfo
    {
        public string ForecastType { get; set; }

        [JsonPropertyName("revenueForecastID")]
        public new string RevenueForecastId { get; set; }
        
    }
}