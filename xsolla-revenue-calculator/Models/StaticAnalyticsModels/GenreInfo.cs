using System.Collections.Generic;

namespace xsolla_revenue_calculator.Models.StaticAnalyticsModels
{
    public class GenreInfo
    {
        public string Genre { get; set; }
        public List<RegionInfo> RegionsInfo { get; set; }
    }
}