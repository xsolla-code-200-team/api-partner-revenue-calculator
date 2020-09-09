using System.Threading.Tasks;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.Services.UserLoggingService
{
    public interface IUserLoggingService
    {
        Task<UserInfo> LogUserAsync(UserInfoRequestBody userInfoRequestBody);
    }
}