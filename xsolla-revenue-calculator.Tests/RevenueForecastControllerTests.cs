using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using xsolla_revenue_calculator.Controllers;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Models.ForecastModels;
using xsolla_revenue_calculator.Models.UserInfoModels;
using xsolla_revenue_calculator.Services;
using xsolla_revenue_calculator.Utilities;
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
        public async void PostUserComplex_Ok()
        {
            // Arrange
            var mockLoggingService = new Mock<IDatabaseAccessService>();
            mockLoggingService.Setup(service => service.LogUserAsync(It.IsAny<UserInfoFullRequestBody>()))
                .ReturnsAsync(new FullUserInfo());
            var mockRevenueForecastService = new Mock<IRevenueForecastService>();
            mockRevenueForecastService.Setup(service => service.StartCalculationAsync(It.IsAny<FullUserInfo>()))
                .ReturnsAsync(new RevenueForecasts());
            var controller = new RevenueForecastController(mockLoggingService.Object, mockRevenueForecastService.Object, _mapper);
            
            // Act
            var requestBody = new UserInfoFullRequestBody
            {
                Email = "123"
            };
            var result = await controller.PostUserComplexAsync(requestBody);
            
            // Assert
            if (result is OkObjectResult objectResult)
            {
                Assert.IsType<RevenueForecastViewModel>(objectResult.Value as RevenueForecastViewModel);
            }
            else Assert.True(false, "failed to receive revenue forecast as result");
        }
        
        [Fact]
        public async void PostUserSimple_Ok()
        {
            // Arrange
            var mockLoggingService = new Mock<IDatabaseAccessService>();
            mockLoggingService.Setup(service => service.LogUserAsync(It.IsAny<UserInfoBaseRequestBody>()))
                .ReturnsAsync(new FullUserInfo());
            var mockRevenueForecastService = new Mock<IRevenueForecastService>();
            mockRevenueForecastService.Setup(service => service.StartCalculationAsync(It.IsAny<FullUserInfo>()))
                .ReturnsAsync(new RevenueForecasts());
            var controller = new RevenueForecastController(mockLoggingService.Object, mockRevenueForecastService.Object, _mapper);
            
            // Act
            var requestBody = new UserInfoBaseRequestBody
            {
                Email = "123"
            };
            var result = await controller.PostUserSimpleAsync(requestBody);
            
            // Assert
            if (result is OkObjectResult objectResult)
            {
                Assert.IsType<RevenueForecastViewModel>(objectResult.Value as RevenueForecastViewModel);
            }
            else Assert.True(false, "failed to receive revenue forecast as result");
        }
        
        public async void GetRevenueForecast_Ok()
        {
            // Arrange
            var mockLoggingService = new Mock<IDatabaseAccessService>();
            mockLoggingService.Setup(service => service.LogUserAsync(It.IsAny<UserInfoBaseRequestBody>()))
                .ReturnsAsync(new FullUserInfo());
            var mockRevenueForecastService = new Mock<IRevenueForecastService>();
            mockRevenueForecastService.Setup(service => service.StartCalculationAsync(It.IsAny<FullUserInfo>()))
                .ReturnsAsync(new RevenueForecasts());
            var controller = new RevenueForecastController(mockLoggingService.Object, mockRevenueForecastService.Object, _mapper);
            
            // Act
            var requestBody = new UserInfoBaseRequestBody
            {
                Email = "123"
            };
            var result = await controller.PostUserSimpleAsync(requestBody);
            
            // Assert
            if (result is OkObjectResult objectResult)
            {
                Assert.IsType<RevenueForecastViewModel>(objectResult.Value as RevenueForecastViewModel);
            }
            else Assert.True(false, "failed to receive revenue forecast as result");
        }
    }
}