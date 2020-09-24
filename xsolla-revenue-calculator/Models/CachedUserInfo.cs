using System;
using System.Collections.Generic;
using System.Linq;

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

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is CachedUserInfo))
            {
                return false;
            }
            return Equals(obj as CachedUserInfo);
        }

        private bool Equals(CachedUserInfo other)
        {
            return ForecastType == other.ForecastType && ReleaseDate == other.ReleaseDate && Genres.SequenceEqual(other.Genres) && Monetization == other.Monetization && Platforms.SequenceEqual(other.Platforms) && Regions.SequenceEqual(other.Regions) && Sales == other.Sales && Cost == other.Cost;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) ForecastType, ReleaseDate, Genres, Monetization, Platforms, Regions, Sales, Cost);
        }
    }
    
}