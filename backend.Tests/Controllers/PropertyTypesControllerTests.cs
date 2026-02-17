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
    public class PropertyTypesControllerTests
    {
        private Mock<IPropertyTypeService> _propertyTypeServiceMock = null!;
        private PropertyTypesController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _propertyTypeServiceMock = new Mock<IPropertyTypeService>();
            _controller = new PropertyTypesController(_propertyTypeServiceMock.Object);
        }

        [Test]
        public async Task GetAllPropertyTypes_ShouldReturn200OK_WithPropertyTypes()
        {
            // Arrange
            var propertyTypeDtos = new List<PropertyTypeDto>
            {
                new PropertyTypeDto { Id = 1, Type = "Apartment" },
                new PropertyTypeDto { Id = 2, Type = "House" },
                new PropertyTypeDto { Id = 3, Type = "Commercial" }
            };

            _propertyTypeServiceMock.Setup(s => s.GetAllPropertyTypesAsync())
                .ReturnsAsync(propertyTypeDtos);

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
            var emptyList = new List<PropertyTypeDto>();

            _propertyTypeServiceMock.Setup(s => s.GetAllPropertyTypesAsync())
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
