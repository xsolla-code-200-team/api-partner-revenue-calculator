using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using xsolla_revenue_calculator.Controllers;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Services.DatabaseAccessService;
using xsolla_revenue_calculator.Services.RevenueForecastService;
using xsolla_revenue_calculator.Utilities;
using xsolla_revenue_calculator.ViewModels;
using Xunit;
using Xunit.Sdk;

namespace xsolla_revenue_calculator.Tests
{
    public class RevenueForecastControllerTests
    {
        private IMapper _mapper
        {
            get
            {
                return new Mapper(new MapperConfiguration(config =>
                {
                    config.AddProfile<AutoMapperProfile>();
                }));
            }
            
        }

        [Fact]
        public async void PostUserInfo_Ok()
        {
            // Arrange
            var mockLoggingService = new Mock<IDatabaseAccessService>();
            mockLoggingService.Setup(service => service.LogUserAsync(It.IsAny<UserInfo>()))
                .ReturnsAsync(new UserInfo());
            var mockRevenueForecastService = new Mock<IRevenueForecastService>();
            mockRevenueForecastService.Setup(service => service.StartCalculationAsync(It.IsAny<UserInfo>()))
                .ReturnsAsync(new RevenueForecast());
            var controller = new RevenueForecastController(mockLoggingService.Object, mockRevenueForecastService.Object, _mapper);
            
            // Act
            var requestBody = new UserInfo
            {
                Email = "123"
            };
            var result = await controller.PostUserInfoAsync(requestBody);
            
            // Assert
            if (result is OkObjectResult objectResult)
            {
                Assert.IsType<RevenueForecastViewModel>(objectResult.Value as RevenueForecastViewModel);
            }
            else Assert.True(false, "failed to receive UserInfo as result");
        }
    }
}