
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

namespace xsolla_revenue_calculator.Models.UserInfoModels
{
    public class UserInfoFullRequestBody : FullUserInfo
    {
        
    }
    
    public class UserInfoFullRequestBodyExample : IExamplesProvider<UserInfoFullRequestBody>
    {
        public UserInfoFullRequestBody GetExamples()
        {
            return new UserInfoFullRequestBody
            {
                ProductName = "Super game",
                Genres = new List<string> {"rpg", "action"},
                Monetization = "free2play",
                Platforms = new List<string> {"macos", "ios"},
                Regions = new List<string> {"1", "5", "10"},
                CompanyName = "Super developer",
                Email = "super-developer@email.com",
                Cost = 100000,
                Sales = 50,
                ReleaseDate = "April-June"
            };
        }
    }
}