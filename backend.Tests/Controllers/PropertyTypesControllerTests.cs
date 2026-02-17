using NUnit.Framework;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BackendApi.Controllers;
using BackendApi.Interfaces;
using BackendApi.Models;
using BackendApi.Models.DTOs;

namespace BackendApi.Tests.Controllers
{
    [TestFixture]
    public class PropertyTypesControllerTests
    {
        private Mock<IPropertyTypeRepository> _propertyTypeRepositoryMock = null!;
        private PropertyTypesController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _propertyTypeRepositoryMock = new Mock<IPropertyTypeRepository>();
            _controller = new PropertyTypesController(_propertyTypeRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllPropertyTypes_ShouldReturn200OK_WithPropertyTypes()
        {
            // Arrange
            var propertyTypes = new List<PropertyType>
            {
                new PropertyType { Id = 1, Type = "Apartment" },
                new PropertyType { Id = 2, Type = "House" },
                new PropertyType { Id = 3, Type = "Commercial" }
            };

            _propertyTypeRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(propertyTypes);

            // Act
            var result = await _controller.GetAllPropertyTypes();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            
            var returnedPropertyTypes = okResult.Value as IEnumerable<PropertyTypeDto>;
            returnedPropertyTypes.Should().NotBeNull();
            returnedPropertyTypes.Should().HaveCount(3);
            returnedPropertyTypes.Should().Contain(pt => pt.Id == 1 && pt.Type == "Apartment");
            returnedPropertyTypes.Should().Contain(pt => pt.Id == 2 && pt.Type == "House");
            returnedPropertyTypes.Should().Contain(pt => pt.Id == 3 && pt.Type == "Commercial");
        }

        [Test]
        public async Task GetAllPropertyTypes_ShouldReturn200OK_WithEmptyList_WhenNoPropertyTypes()
        {
            // Arrange
            var emptyList = new List<PropertyType>();

            _propertyTypeRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetAllPropertyTypes();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            
            var returnedPropertyTypes = okResult.Value as IEnumerable<PropertyTypeDto>;
            returnedPropertyTypes.Should().NotBeNull();
            returnedPropertyTypes.Should().BeEmpty();
        }
    }
}
