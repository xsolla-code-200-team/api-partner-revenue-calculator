using System.Collections.Generic;

namespace xsolla_revenue_calculator.Models.ForecastModels
{
    public class ForecastsInfo
    {
        public ParticularForecast ChosenForecast { get; set; }
        
        public List<ParticularForecast> OtherForecasts { get; set; } 
    }
}