using System.Linq;
using AutoMapper;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.DTO.MqMessages;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Models.ForecastModels;
using xsolla_revenue_calculator.Models.UserInfoModels;

namespace xsolla_revenue_calculator.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RevenueForecasts, RevenueForecastViewModel>()
                .ForMember(viewModel => viewModel.Id, opt =>
                    opt.MapFrom(src => src.Id.ToString()))
                .ForMember(viewModel => viewModel.ForecastType, opt => 
                    opt.MapFrom(src => src.ForecastType.ToString()));
            CreateMap<FullUserInfo, MessageToModel>();
            CreateMap<UserInfoFullRequestBody, FullUserInfo>();
            CreateMap<UserInfoBaseRequestBody, FullUserInfo>();
            CreateMap<FullUserInfo, CachedUserInfo>();
        }
    }
}