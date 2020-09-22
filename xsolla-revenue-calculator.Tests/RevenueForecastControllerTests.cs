using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using xsolla_revenue_calculator.Controllers;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Services;
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
        public async void PostUserComplex_Ok()
        {
            // Arrange
            var mockLoggingService = new Mock<IDatabaseAccessService>();
            mockLoggingService.Setup(service => service.LogUserAsync(It.IsAny<UserComplexFormDto>()))
                .ReturnsAsync(new UserInfo());
            var mockRevenueForecastService = new Mock<IRevenueForecastService>();
            mockRevenueForecastService.Setup(service => service.StartCalculationAsync(It.IsAny<UserInfo>()))
                .ReturnsAsync(new RevenueForecasts());
            var controller = new RevenueForecastController(mockLoggingService.Object, mockRevenueForecastService.Object, _mapper);
            
            // Act
            var requestBody = new UserComplexFormDto
            {
                Email = "123"
            };
            var result = await controller.PostUserComplexAsync(requestBody);
            
            // Assert
            if (result is OkObjectResult objectResult)
            {
                Assert.IsType<RevenueForecastViewModel>(objectResult.Value as RevenueForecastViewModel);
            }
            else Assert.True(false, "failed to receive UserInfo as result");
        }
    }
}