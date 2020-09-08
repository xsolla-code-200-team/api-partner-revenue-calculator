using AutoMapper;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;

namespace xsolla_revenue_calculator.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserInfoRequestBody, UserInfo>();
        }
    }
}