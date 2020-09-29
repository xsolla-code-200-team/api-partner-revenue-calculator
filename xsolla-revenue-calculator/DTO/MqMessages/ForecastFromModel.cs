using System;
using System.Collections.Generic;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Models.ForecastModels;

namespace xsolla_revenue_calculator.DTO.MqMessages
{
    /// <summary>
    /// To use for receiving messages from model via RabbitMQ
    /// </summary>
    public class ForecastFromModel : ForecastsInfo
    {
        public string RevenueForecastId { get; set; }

        public override string ToString()
        {
            var result = $"RevenueForecastID: {RevenueForecastId}, ChosenForecast:\n" +
                         $"Monetization: {ChosenForecast.Monetization}\n" +
                         $"TendencyForecast: {String.Join(' ', ChosenForecast.TendencyForecast)}\n" +
                         $"CumulativeForecast {String.Join(' ', ChosenForecast.CumulativeForecast)}\n";
            foreach (var forecast in OtherForecasts)
            {
                result += $"Other forecast:\n" +
                          $"Monetization: {forecast.Monetization}\n" +
                          $"TendencyForecast: {String.Join(' ', forecast.TendencyForecast)}\n" +
                          $"CumulativeForecast {String.Join(' ', forecast.CumulativeForecast)}\n";
            }

            return result;
        }
    }
}