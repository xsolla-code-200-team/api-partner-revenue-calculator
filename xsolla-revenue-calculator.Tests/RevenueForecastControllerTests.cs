using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using xsolla_revenue_calculator.Controllers;
using xsolla_revenue_calculator.DTO;
using xsolla_revenue_calculator.Models;
using xsolla_revenue_calculator.Services.ModelController;
using xsolla_revenue_calculator.Services.UserLoggingService;
using Xunit;
using Xunit.Sdk;

namespace xsolla_revenue_calculator.Tests
{
    public class RevenueForecastControllerTests
    {
        [Fact]
        public async void PostUserInfo_Ok()
        {
            // Arrange
            var mockLoggingService = new Mock<IUserLoggingService>();
            mockLoggingService.Setup(service => service.LogUserAsync(It.IsAny<UserInfoRequestBody>()))
                .ReturnsAsync(new UserInfo());
            var mockModelControllerService = new Mock<IModelController>();
            mockModelControllerService.Setup(service => service.Publish(It.IsAny<string>()));
            var controller = new RevenueForecastController(mockLoggingService.Object, mockModelControllerService.Object);
            // Act
            var requestBody = new UserInfoRequestBody
            {
                Email = "123"
            };
            var result = await controller.PostUserInfoAsync(requestBody);
            // Assert
            if (result is OkObjectResult objectResult)
            {
                Assert.IsType<UserInfo>(objectResult.Value as UserInfo);
            }
            else Assert.True(false, "failed to receive UserInfo as result");
        }
    }
}