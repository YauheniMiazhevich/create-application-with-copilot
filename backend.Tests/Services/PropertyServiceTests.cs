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
    public class PropertyServiceTests
    {
        private Mock<IPropertyRepository> _propertyRepositoryMock = null!;
        private Mock<IOwnerRepository> _ownerRepositoryMock = null!;
        private Mock<IPropertyTypeRepository> _propertyTypeRepositoryMock = null!;
        private PropertyService _service = null!;

        [SetUp]
        public void Setup()
        {
            _propertyRepositoryMock = new Mock<IPropertyRepository>();
            _ownerRepositoryMock = new Mock<IOwnerRepository>();
            _propertyTypeRepositoryMock = new Mock<IPropertyTypeRepository>();
            _service = new PropertyService(
                _propertyRepositoryMock.Object,
                _ownerRepositoryMock.Object,
                _propertyTypeRepositoryMock.Object);
        }

        [Test]
        public void CreatePropertyAsync_ShouldThrowException_WhenOwnerNotFound()
        {
            // Arrange
            var createDto = new CreatePropertyDto
            {
                OwnerId = 999,
                PropertyTypeId = 1,
                PropertyLength = 100m,
                PropertyCost = 250000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Country = "USA",
                City = "New York"
            };

            _ownerRepositoryMock.Setup(r => r.ExistsAsync(999))
                .ReturnsAsync(false);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _service.CreatePropertyAsync(createDto));
        }

        [Test]
        public void CreatePropertyAsync_ShouldThrowException_WhenPropertyTypeNotFound()
        {
            // Arrange
            var createDto = new CreatePropertyDto
            {
                OwnerId = 1,
                PropertyTypeId = 999,
                PropertyLength = 100m,
                PropertyCost = 250000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Country = "USA",
                City = "New York"
            };

            _ownerRepositoryMock.Setup(r => r.ExistsAsync(1))
                .ReturnsAsync(true);

            _propertyTypeRepositoryMock.Setup(r => r.ExistsAsync(999))
                .ReturnsAsync(false);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _service.CreatePropertyAsync(createDto));
        }

        [Test]
        public async Task CreatePropertyAsync_ShouldReturnDto_WhenValid()
        {
            // Arrange
            var createDto = new CreatePropertyDto
            {
                OwnerId = 1,
                PropertyTypeId = 1,
                PropertyLength = 100m,
                PropertyCost = 250000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Description = "Beautiful house",
                Country = "USA",
                City = "New York",
                Street = "5th Avenue",
                ZipCode = "10001"
            };

            var createdProperty = new Property
            {
                Id = 1,
                OwnerId = 1,
                PropertyTypeId = 1,
                PropertyLength = 100m,
                PropertyCost = 250000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Description = "Beautiful house",
                Country = "USA",
                City = "New York",
                Street = "5th Avenue",
                ZipCode = "10001",
                Owner = new Owner 
                { 
                    Id = 1, 
                    FirstName = "John", 
                    LastName = "Doe",
                    Email = "john@example.com",
                    Phone = "+1234567890"
                },
                PropertyType = new PropertyType { Id = 1, Type = "residential" }
            };

            _ownerRepositoryMock.Setup(r => r.ExistsAsync(1))
                .ReturnsAsync(true);

            _propertyTypeRepositoryMock.Setup(r => r.ExistsAsync(1))
                .ReturnsAsync(true);

            _propertyRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Property>()))
                .ReturnsAsync((Property p) => { p.Id = 1; return p; });

            _propertyRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(createdProperty);

            // Act
            var result = await _service.CreatePropertyAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.PropertyCost.Should().Be(250000m);
            result.Owner.Should().NotBeNull();
            result.PropertyType.Should().NotBeNull();
        }

        [Test]
        public async Task GetPropertyByIdAsync_ShouldReturnDto_WhenExists()
        {
            // Arrange
            var property = new Property
            {
                Id = 1,
                OwnerId = 1,
                PropertyTypeId = 2,
                PropertyLength = 500m,
                PropertyCost = 1000000m,
                DateOfBuilding = new DateTime(2015, 6, 15),
                Country = "UK",
                City = "London",
                Owner = new Owner 
                { 
                    Id = 1, 
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane@example.com",
                    Phone = "+0987654321"
                },
                PropertyType = new PropertyType { Id = 2, Type = "commercial" }
            };

            _propertyRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(property);

            // Act
            var result = await _service.GetPropertyByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.PropertyCost.Should().Be(1000000m);
            result.PropertyType!.Type.Should().Be("commercial");
        }

        [Test]
        public async Task UpdatePropertyAsync_ShouldReturnUpdatedDto()
        {
            // Arrange
            var existingProperty = new Property
            {
                Id = 1,
                OwnerId = 1,
                PropertyTypeId = 1,
                PropertyLength = 100m,
                PropertyCost = 200000m,
                DateOfBuilding = new DateTime(2010, 1, 1),
                Country = "USA",
                City = "Original City",
                Owner = new Owner { Id = 1, FirstName = "Bob", LastName = "Johnson",Email = "bob@example.com", Phone = "+1111111111" },
                PropertyType = new PropertyType { Id = 1, Type = "residential" }
            };

            var updateDto = new UpdatePropertyDto
            {
                PropertyCost = 250000m,
                City = "Updated City"
            };

            _propertyRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingProperty);

            _propertyRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Property>()))
                .ReturnsAsync((Property p) => p);

            // Act
            var result = await _service.UpdatePropertyAsync(1, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.PropertyCost.Should().Be(250000m);
            result.City.Should().Be("Updated City");
        }

        [Test]
        public async Task DeletePropertyAsync_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            _propertyRepositoryMock.Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeletePropertyAsync(1);

            // Assert
            result.Should().BeTrue();
        }
    }
}
