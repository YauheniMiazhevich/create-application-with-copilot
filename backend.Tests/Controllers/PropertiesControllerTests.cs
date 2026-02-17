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
    public class PropertiesControllerTests
    {
        private Mock<IPropertyService> _propertyServiceMock = null!;
        private PropertiesController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _propertyServiceMock = new Mock<IPropertyService>();
            _controller = new PropertiesController(_propertyServiceMock.Object);
        }

        [Test]
        public async Task GetAllProperties_ShouldReturn200OK_WithProperties()
        {
            // Arrange
            var propertyDtos = new List<PropertyDto>
            {
                new PropertyDto
                {
                    Id = 1,
                    OwnerId = 1,
                    PropertyTypeId = 1,
                    PropertyLength = 100m,
                    PropertyCost = 250000m,
                    DateOfBuilding = new DateTime(2020, 1, 1),
                    Country = "USA",
                    City = "New York",
                    Owner = new OwnerDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "+1234567890" },
                    PropertyType = new PropertyTypeDto { Id = 1, Type = "residential" }
                },
                new PropertyDto
                {
                    Id = 2,
                    OwnerId = 2,
                    PropertyTypeId = 2,
                    PropertyLength = 500m,
                    PropertyCost = 1000000m,
                    DateOfBuilding = new DateTime(2015, 6, 15),
                    Country = "UK",
                    City = "London",
                    Owner = new OwnerDto { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Phone = "+0987654321" },
                    PropertyType = new PropertyTypeDto { Id = 2, Type = "commercial" }
                }
            };

            _propertyServiceMock.Setup(s => s.GetAllPropertiesAsync())
                .ReturnsAsync(propertyDtos);

            // Act
            var result = await _controller.GetAllProperties();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            
            var returnedProperties = okResult.Value as IEnumerable<PropertyDto>;
            returnedProperties.Should().NotBeNull();
            returnedProperties.Should().HaveCount(2);
            returnedProperties.Should().Contain(p => p.Id == 1 && p.City == "New York");
            returnedProperties.Should().Contain(p => p.Id == 2 && p.City == "London");
        }

        [Test]
        public async Task GetAllProperties_ShouldReturn200OK_WithEmptyList_WhenNoProperties()
        {
            // Arrange
            var emptyList = new List<PropertyDto>();

            _propertyServiceMock.Setup(s => s.GetAllPropertiesAsync())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetAllProperties();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            
            var returnedProperties = okResult.Value as IEnumerable<PropertyDto>;
            returnedProperties.Should().NotBeNull();
            returnedProperties.Should().BeEmpty();
        }

        [Test]
        public async Task CreateProperty_ShouldReturn201Created_WhenValid()
        {
            // Arrange
            var createDto = new CreatePropertyDto
            {
                OwnerId = 1,
                PropertyTypeId = 1,
                PropertyLength = 100m,
                PropertyCost = 250000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Country = "USA",
                City = "New York"
            };

            var propertyDto = new PropertyDto
            {
                Id = 1,
                OwnerId = 1,
                PropertyTypeId = 1,
                PropertyLength = 100m,
                PropertyCost = 250000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Country = "USA",
                City = "New York",
                Owner = new OwnerDto 
                { 
                    Id = 1, 
                    FirstName = "John", 
                    LastName = "Doe",
                    Email = "john@example.com",
                    Phone = "+1234567890"
                },
                PropertyType = new PropertyTypeDto { Id = 1, Type = "residential" }
            };

            _propertyServiceMock.Setup(s => s.CreatePropertyAsync(createDto))
                .ReturnsAsync(propertyDto);

            // Act
            var result = await _controller.CreateProperty(createDto);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(201);
            createdResult.Value.Should().BeEquivalentTo(propertyDto);
        }

        [Test]
        public async Task GetProperty_ShouldReturn200OK_WhenExists()
        {
            // Arrange
            var propertyDto = new PropertyDto
            {
                Id = 1,
                OwnerId = 1,
                PropertyTypeId = 2,
                PropertyLength = 500m,
                PropertyCost = 1000000m,
                DateOfBuilding = new DateTime(2015, 6, 15),
                Country = "UK",
                City = "London",
                Owner = new OwnerDto
                {
                    Id = 1,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane@example.com",
                    Phone = "+0987654321"
                },
                PropertyType = new PropertyTypeDto { Id = 2, Type = "commercial" }
            };

            _propertyServiceMock.Setup(s => s.GetPropertyByIdAsync(1))
                .ReturnsAsync(propertyDto);

            // Act
            var result = await _controller.GetProperty(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(propertyDto);
        }

        [Test]
        public async Task GetProperty_ShouldReturn404NotFound_WhenNotExists()
        {
            // Arrange
            _propertyServiceMock.Setup(s => s.GetPropertyByIdAsync(999))
                .ReturnsAsync((PropertyDto?)null);

            // Act
            var result = await _controller.GetProperty(999);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }



        [Test]
        public async Task UpdateProperty_ShouldReturn200OK_WhenValid()
        {
            // Arrange
            var updateDto = new UpdatePropertyDto
            {
                PropertyCost = 300000m,
                City = "Updated City"
            };

            var updatedProperty = new PropertyDto
            {
                Id = 1,
                OwnerId = 1,
                PropertyTypeId = 1,
                PropertyLength = 100m,
                PropertyCost = 300000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Country = "USA",
                City = "Updated City"
            };

            _propertyServiceMock.Setup(s => s.UpdatePropertyAsync(1, updateDto))
                .ReturnsAsync(updatedProperty);

            // Act
            var result = await _controller.UpdateProperty(1, updateDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(updatedProperty);
        }

        [Test]
        public async Task UpdateProperty_ShouldReturn404NotFound_WhenNotExists()
        {
            // Arrange
            var updateDto = new UpdatePropertyDto { PropertyCost = 300000m };

            _propertyServiceMock.Setup(s => s.UpdatePropertyAsync(999, updateDto))
                .ReturnsAsync((PropertyDto?)null);

            // Act
            var result = await _controller.UpdateProperty(999, updateDto);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public async Task DeleteProperty_ShouldReturn204NoContent_WhenSuccessful()
        {
            // Arrange
            _propertyServiceMock.Setup(s => s.DeletePropertyAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProperty(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task DeleteProperty_ShouldReturn404NotFound_WhenNotExists()
        {
            // Arrange
            _propertyServiceMock.Setup(s => s.DeletePropertyAsync(999))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProperty(999);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
