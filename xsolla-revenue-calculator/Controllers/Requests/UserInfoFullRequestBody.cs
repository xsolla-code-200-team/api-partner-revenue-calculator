using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using xsolla_revenue_calculator.Models.UserInfoModels;

namespace xsolla_revenue_calculator.Controllers.Requests
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
                Platforms = new List<string> {"mac", "ios"},
                Regions = new List<string> {"1", "10"},
                CompanyName = "Super developer",
                Email = "super-developer@email.com",
                InitialRevenue = 50,
                ReleaseDate = "april-june"
            };
        }
    }
}