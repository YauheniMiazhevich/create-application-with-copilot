using NUnit.Framework;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BackendApi.Controllers;
using BackendApi.Interfaces;
using BackendApi.Models.DTOs;

namespace BackendApi.Tests.Controllers
{
    [TestFixture]
    public class OwnersControllerTests
    {
        private Mock<IOwnerService> _ownerServiceMock = null!;
        private OwnersController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _ownerServiceMock = new Mock<IOwnerService>();
            _controller = new OwnersController(_ownerServiceMock.Object);
        }

        [Test]
        public async Task CreateOwner_ShouldReturn201Created_WhenValid()
        {
            // Arrange
            var createDto = new CreateOwnerDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "+1234567890"
            };

            var ownerDto = new OwnerDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "+1234567890",
                IsCompanyContact = false
            };

            _ownerServiceMock.Setup(s => s.CreateOwnerAsync(createDto))
                .ReturnsAsync(ownerDto);

            // Act
            var result = await _controller.CreateOwner(createDto);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(201);
            createdResult.Value.Should().BeEquivalentTo(ownerDto);
        }

        [Test]
        public async Task GetOwner_ShouldReturn200OK_WhenExists()
        {
            // Arrange
            var ownerDto = new OwnerDto
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                Phone = "+0987654321",
                IsCompanyContact = true
            };

            _ownerServiceMock.Setup(s => s.GetOwnerByIdAsync(1))
                .ReturnsAsync(ownerDto);

            // Act
            var result = await _controller.GetOwner(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(ownerDto);
        }

        [Test]
        public async Task GetOwner_ShouldReturn404NotFound_WhenNotExists()
        {
            // Arrange
            _ownerServiceMock.Setup(s => s.GetOwnerByIdAsync(999))
                .ReturnsAsync((OwnerDto?)null);

            // Act
            var result = await _controller.GetOwner(999);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }



        [Test]
        public async Task UpdateOwner_ShouldReturn200OK_WhenValid()
        {
            // Arrange
            var updateDto = new UpdateOwnerDto
            {
                FirstName = "Updated",
                Phone = "+9999999999"
            };

            var updatedOwner = new OwnerDto
            {
                Id = 1,
                FirstName = "Updated",
                LastName = "Original",
                Email = "original@example.com",
                Phone = "+9999999999",
                IsCompanyContact = false
            };

            _ownerServiceMock.Setup(s => s.UpdateOwnerAsync(1, updateDto))
                .ReturnsAsync(updatedOwner);

            // Act
            var result = await _controller.UpdateOwner(1, updateDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(updatedOwner);
        }

        [Test]
        public async Task UpdateOwner_ShouldReturn404NotFound_WhenNotExists()
        {
            // Arrange
            var updateDto = new UpdateOwnerDto { FirstName = "Updated" };

            _ownerServiceMock.Setup(s => s.UpdateOwnerAsync(999, updateDto))
                .ReturnsAsync((OwnerDto?)null);

            // Act
            var result = await _controller.UpdateOwner(999, updateDto);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public async Task DeleteOwner_ShouldReturn204NoContent_WhenSuccessful()
        {
            // Arrange
            _ownerServiceMock.Setup(s => s.DeleteOwnerAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteOwner(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task DeleteOwner_ShouldReturn409Conflict_WhenHasDependencies()
        {
            // Arrange
            _ownerServiceMock.Setup(s => s.DeleteOwnerAsync(1))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteOwner(1);

            // Assert
            var conflictResult = result as ObjectResult;
            conflictResult.Should().NotBeNull();
            conflictResult!.StatusCode.Should().Be(409);
        }
    }
}
