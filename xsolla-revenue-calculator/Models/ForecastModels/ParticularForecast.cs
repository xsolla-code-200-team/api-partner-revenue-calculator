using System.Collections.Generic;

namespace xsolla_revenue_calculator.Models.ForecastModels
{
    public class ParticularForecast
    {
        public string Monetization { get; set; }
        public List<double> Forecast { get; set; }
    }
}