using System.Collections.Generic;
using MongoDB.Bson;

namespace xsolla_revenue_calculator.Models
{
    public class RevenueForecast
    {
        public ObjectId Id { get; set; }
        public bool IsReady { get; set; }
        
        public List<double> RevenuePerMonth { get; set; }
    }
}