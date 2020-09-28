using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Models.UserInfoModels;

namespace xsolla_revenue_calculator.Services.CachingService
{
    public class HashingService : IHashingService
    {
        public async Task<string> GetHash(CachedUserInfo userInfo)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(userInfo));
            using var md5 = MD5.Create();
            return Encoding.Default.GetString(await Task.Run(() => md5.ComputeHash(bytes)));
        }
    }

    public interface IHashingService
    {
        Task<string> GetHash(CachedUserInfo userInfo);
    }
}