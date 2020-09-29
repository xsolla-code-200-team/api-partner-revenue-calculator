using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

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

    public class RevenueForecastViewModelExample : IExamplesProvider<RevenueForecastViewModel>
    {
        public RevenueForecastViewModel GetExamples()
        {
            return new RevenueForecastViewModel
            {
                Id = "5f6368d4933cfe309b7a08b7",
                IsReady = true,
                ForecastType = ForecastType.Absolute.ToString(),
                ChosenForecast = new ParticularForecast
                {
                    Monetization = "free2play",
                    TendencyForecast = new List<double> {0.1, 0.2, 0.3, 0.4, 0.5, 0.6},
                    CumulativeForecast = new List<double> {0.1, 0.3, 0.6, 1.1, 1.7}
                },
                OtherForecasts = new List<ParticularForecast>
                {
                    new ParticularForecast
                    {
                        Monetization = "pay2play",
                        TendencyForecast = new List<double> {0.2, 0.3, 0.4, 0.5, 0.6},
                        CumulativeForecast = new List<double> {0.2, 0.5, 0.9, 1.4, 2.0}
                    }
                },
            };
        }
    }
}