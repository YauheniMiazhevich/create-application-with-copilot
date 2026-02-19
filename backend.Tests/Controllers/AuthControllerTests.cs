using NUnit.Framework;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using BackendApi.Controllers;
using BackendApi.Models;

namespace BackendApi.Tests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private AuthController _controller = null!;

        [SetUp]
        public void Setup()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var contextAccessorMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                userManagerMock.Object,
                contextAccessorMock.Object,
                claimsFactoryMock.Object,
                null!, null!, null!, null!);

            var configurationMock = new Mock<IConfiguration>();

            _controller = new AuthController(
                userManagerMock.Object,
                signInManagerMock.Object,
                configurationMock.Object);
        }

        [Test]
        public void Logout_ShouldReturn200OK_WhenAuthorized()
        {
            // Act
            var result = _controller.Logout();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
        }

        [Test]
        public void Logout_ShouldReturnSuccessMessage()
        {
            // Act
            var result = _controller.Logout() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            var value = result!.Value;
            value.Should().NotBeNull();

            var messageProperty = value!.GetType().GetProperty("message");
            messageProperty.Should().NotBeNull();
            messageProperty!.GetValue(value).Should().Be("Logged out successfully");
        }
    }
}
