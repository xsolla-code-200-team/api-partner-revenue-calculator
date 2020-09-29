using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using xsolla_revenue_calculator.Models.UserInfoModels;

namespace xsolla_revenue_calculator.Controllers.Requests
{
    public class UserInfoBaseRequestBody : BaseUserInfo
    {
    }

    public class UserInfoBaseRequestBodyExample : IExamplesProvider<UserInfoBaseRequestBody>
    {
        public UserInfoBaseRequestBody GetExamples()
        {
            return new UserInfoBaseRequestBody
            {
                ProductName = "Super game",
                Genres = new List<string> {"rpg", "action"},
                Monetization = "free2play",
                Platforms = new List<string> {"mac", "ios"},
                Regions = new List<string> {"1", "10"},
                CompanyName = "Super developer",
                Email = "super-developer@email.com"
            };
        }
    }
}