using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Models.UserInfoModels;

namespace xsolla_revenue_calculator.Services.CachingService
{
    public class HashingService : IHashingService
    {
        public string GetHash(CachedUserInfo userInfo)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(userInfo));
            using var md5 = MD5.Create();
            return Encoding.Default.GetString(md5.ComputeHash(bytes)) ?? throw new InvalidOperationException();
        }
    }

    public interface IHashingService
    {
        string GetHash(CachedUserInfo userInfo);
    }
}