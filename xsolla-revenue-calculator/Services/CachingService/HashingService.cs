using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.Services.CachingService
{
    public class HashingService : IHashingService
    {
        public string GetHash(CachedUserInfo userInfo)
        {
            return "";
        }
    }

    public interface IHashingService
    {
        string GetHash(CachedUserInfo userInfo);
    }
}