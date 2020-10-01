using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using xsolla_revenue_calculator.Models.ForecastModels;

namespace xsolla_revenue_calculator.Models.UserInfoModels
{
    public class CachedUserInfo
    {
        public ForecastType ForecastType { get; set; }

        public string ReleaseDate { get; set; }
        
        public List<string> Genres { get; set; }
        
        public string Monetization { get; set; }
        
        public List<string> Platforms { get; set; }
        
        public List<string> Regions { get; set; }
        
        [JsonPropertyName("initialRevenue")]
        public double InitialRevenue { get; set; }
        
        [JsonPropertyName("isReleased")]
        public bool IsReleased { get; set; }

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
            return ForecastType == other.ForecastType && ReleaseDate == other.ReleaseDate && Genres.SequenceEqual(other.Genres) && Monetization == other.Monetization && Platforms.SequenceEqual(other.Platforms) && Regions.SequenceEqual(other.Regions) && InitialRevenue == other.InitialRevenue && IsReleased == other.IsReleased;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) ForecastType, ReleaseDate, Genres, Monetization, Platforms, Regions, InitialRevenue, IsReleased);
        }
    }
}