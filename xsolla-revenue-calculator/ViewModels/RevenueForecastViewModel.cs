using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.ViewModels
{
    public class RevenueForecastViewModel
    {
        public string Id { get; set; }
        public bool IsReady { get; set; }
        
        public string ForecastType { get; set; }
        
        public ParticularForecast ChosenForecast { get; set; }
        
        public List<ParticularForecast> OtherForecasts { get; set; } 
    }    
}