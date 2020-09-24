using System.Collections.Generic;

namespace xsolla_revenue_calculator.Models
{
    public class CachedUserInfo
    {
        public ForecastType ForecastType { get; set; }

        public string ReleaseDate { get; set; }
        
        public List<string> Genres { get; set; }
        
        public string Monetization { get; set; }
        
        public List<string> Platforms { get; set; }
        
        public List<string> Regions { get; set; }
        
        public string Sales { get; set; }
        
        public string Cost { get; set; }
    }
}