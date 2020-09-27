using System.Collections.Generic;

namespace xsolla_revenue_calculator.Models.ForecastModels
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