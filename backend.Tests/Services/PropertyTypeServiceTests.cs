using NUnit.Framework;
using FluentAssertions;
using Moq;
using BackendApi.Interfaces;
using BackendApi.Models;
using BackendApi.Models.DTOs;
using BackendApi.Services;

namespace BackendApi.Tests.Services
{
    [TestFixture]
    public class PropertyTypeServiceTests
    {
        private Mock<IPropertyTypeRepository> _propertyTypeRepositoryMock = null!;
        private PropertyTypeService _service = null!;

        [SetUp]
        public void Setup()
        {
            _propertyTypeRepositoryMock = new Mock<IPropertyTypeRepository>();
            _service = new PropertyTypeService(_propertyTypeRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllPropertyTypesAsync_ShouldReturnAllPropertyTypeDtos()
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
            var result = await _service.GetAllPropertyTypesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            
            var resultList = result.ToList();
            resultList[0].Id.Should().Be(1);
            resultList[0].Type.Should().Be("Apartment");
            resultList[1].Id.Should().Be(2);
            resultList[1].Type.Should().Be("House");
            resultList[2].Id.Should().Be(3);
            resultList[2].Type.Should().Be("Commercial");
            
            _propertyTypeRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllPropertyTypesAsync_ShouldReturnEmptyList_WhenNoPropertyTypes()
        {
            // Arrange
            var emptyList = new List<PropertyType>();

            _propertyTypeRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _service.GetAllPropertyTypesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _propertyTypeRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllPropertyTypesAsync_ShouldMapPropertiesToDtos()
        {
            // Arrange
            var propertyTypes = new List<PropertyType>
            {
                new PropertyType { Id = 5, Type = "Villa" }
            };

            _propertyTypeRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(propertyTypes);

            // Act
            var result = await _service.GetAllPropertyTypesAsync();

            // Assert
            var dto = result.First();
            dto.Should().BeOfType<PropertyTypeDto>();
            dto.Id.Should().Be(5);
            dto.Type.Should().Be("Villa");
        }
    }
}
