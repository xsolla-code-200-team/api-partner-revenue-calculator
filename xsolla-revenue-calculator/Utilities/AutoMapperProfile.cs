using System.Linq;
using AutoMapper;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.ViewModels;

namespace xsolla_revenue_calculator.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RevenueForecast, RevenueForecastViewModel>()
                .ForMember(viewModel => viewModel.Id, opt => 
                        opt.MapFrom(src => src.Id.ToString()))
                .ForMember(viewModel => viewModel.TotalRevenue, opt => opt.MapFrom(src => src.RevenuePerMonth.Sum()));
            CreateMap<UserInfo, MessageToModel>();
        }
    }
}