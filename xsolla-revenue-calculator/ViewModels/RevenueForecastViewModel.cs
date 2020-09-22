using System.Collections.Generic;

namespace xsolla_revenue_calculator.ViewModels
{
    public class RevenueForecastViewModel
    {
        public string Id { get; set; }
        public bool IsReady { get; set; }
        public List<double> RevenuePerMonth { get; set; } 
    }
}