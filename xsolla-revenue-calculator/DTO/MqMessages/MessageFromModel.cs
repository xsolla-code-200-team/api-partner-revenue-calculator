using System;
using System.Collections.Generic;
using System.Linq;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.DTO
{
    /// <summary>
    /// To use for receiving messages from model via RabbitMQ
    /// </summary>
    public class MessageFromModel
    {
        public string RevenueForecastId { get; set; }
        
        public ParticularForecast ChosenForecast { get; set; }
        
        public List<ParticularForecast> OtherForecasts { get; set; } 
    
        public override string ToString()
        {
            var result = $"RevenueForecastID: {RevenueForecastId}, ChosenForecast:\n" +
                         $"Monetization: {ChosenForecast.Monetization}\n" +
                         $"Forecast: {String.Join(' ', ChosenForecast.Forecast)}\n";
            foreach (var forecast in OtherForecasts)
            {
                result += $"Other forecast:\n" +
                          $"Monetization: {forecast.Monetization}\n" +
                          $"Forecast: {String.Join(' ', forecast.Forecast)}\n";
            }

            return result;
        }
    }
}