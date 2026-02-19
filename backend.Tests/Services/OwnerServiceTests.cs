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
    public class OwnerServiceTests
    {
        private Mock<IOwnerRepository> _ownerRepositoryMock = null!;
        private Mock<ICompanyRepository> _companyRepositoryMock = null!;
        private Mock<IPropertyRepository> _propertyRepositoryMock = null!;
        private OwnerService _service = null!;

        [SetUp]
        public void Setup()
        {
            _ownerRepositoryMock = new Mock<IOwnerRepository>();
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _propertyRepositoryMock = new Mock<IPropertyRepository>();
            _service = new OwnerService(_ownerRepositoryMock.Object, _companyRepositoryMock.Object, _propertyRepositoryMock.Object);
        }

        [Test]
        public async Task CreateOwnerAsync_ShouldReturnOwnerDto()
        {
            // Arrange
            var createDto = new CreateOwnerDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "+1234567890",
                Address = "123 Main St",
                Description = "Test owner"
            };

            var createdOwner = new Owner
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "+1234567890",
                Address = "123 Main St",
                Description = "Test owner",
                IsCompanyContact = false
            };

            _ownerRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Owner>()))
                .ReturnsAsync(createdOwner);

            // Act
            var result = await _service.CreateOwnerAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.FirstName.Should().Be("John");
            result.IsCompanyContact.Should().BeFalse();
            _ownerRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Owner>()), Times.Once);
        }

        [Test]
        public async Task GetOwnerByIdAsync_ShouldReturnDto_WhenExists()
        {
            // Arrange
            var owner = new Owner
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                Phone = "+0987654321"
            };

            _ownerRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(owner);

            // Act
            var result = await _service.GetOwnerByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.FirstName.Should().Be("Jane");
            result.LastName.Should().Be("Smith");
        }

        [Test]
        public async Task GetOwnerByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            _ownerRepositoryMock.Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((Owner?)null);

            // Act
            var result = await _service.GetOwnerByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task UpdateOwnerAsync_ShouldUpdateAndReturnDto()
        {
            // Arrange
            var existingOwner = new Owner
            {
                Id = 1,
                FirstName = "Original",
                LastName = "Name",
                Email = "original@example.com",
                Phone = "+1111111111"
            };

            var updateDto = new UpdateOwnerDto
            {
                FirstName = "Updated",
                Email = "updated@example.com"
            };

            _ownerRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingOwner);

            _ownerRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Owner>()))
                .ReturnsAsync((Owner o) => o);

            // Act
            var result = await _service.UpdateOwnerAsync(1, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.FirstName.Should().Be("Updated");
            result.Email.Should().Be("updated@example.com");
            _ownerRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Owner>()), Times.Once);
        }

        [Test]
        public async Task DeleteOwnerAsync_ShouldReturnFalse_WhenHasCompanies()
        {
            // Arrange
            var companies = new List<Company>
            {
                new Company { Id = 1, OwnerId = 1, CompanyName = "Company1" }
            };

            _companyRepositoryMock.Setup(r => r.GetByOwnerIdAsync(1))
                .ReturnsAsync(companies);

            // Act
            var result = await _service.DeleteOwnerAsync(1);

            // Assert
            result.Should().BeFalse();
            _ownerRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task DeleteOwnerAsync_ShouldReturnFalse_WhenHasProperties()
        {
            // Arrange
            _companyRepositoryMock.Setup(r => r.GetByOwnerIdAsync(1))
                .ReturnsAsync(new List<Company>());

            var properties = new List<Property>
            {
                new Property { Id = 1, OwnerId = 1 }
            };

            _propertyRepositoryMock.Setup(r => r.GetByOwnerIdAsync(1))
                .ReturnsAsync(properties);

            // Act
            var result = await _service.DeleteOwnerAsync(1);

            // Assert
            result.Should().BeFalse();
            _ownerRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task DeleteOwnerAsync_ShouldReturnTrue_WhenNoDependencies()
        {
            // Arrange
            _companyRepositoryMock.Setup(r => r.GetByOwnerIdAsync(1))
                .ReturnsAsync(new List<Company>());

            _propertyRepositoryMock.Setup(r => r.GetByOwnerIdAsync(1))
                .ReturnsAsync(new List<Property>());

            _ownerRepositoryMock.Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteOwnerAsync(1);

            // Assert
            result.Should().BeTrue();
            _ownerRepositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Test]
        public async Task GetAllOwnersAsync_ShouldReturnAllOwners()
        {
            // Arrange
            var owners = new List<Owner>
            {
                new Owner { Id = 1, FirstName = "Owner1", LastName = "Test", Email = "owner1@test.com", Phone = "+1111" },
                new Owner { Id = 2, FirstName = "Owner2", LastName = "Test", Email = "owner2@test.com", Phone = "+2222" }
            };

            _ownerRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(owners);

            // Act
            var result = await _service.GetAllOwnersAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(o => o.FirstName == "Owner1");
            result.Should().Contain(o => o.FirstName == "Owner2");
        }
    }
}
