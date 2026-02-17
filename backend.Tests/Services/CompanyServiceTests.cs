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
    public class CompanyServiceTests
    {
        private Mock<ICompanyRepository> _companyRepositoryMock = null!;
        private Mock<IOwnerRepository> _ownerRepositoryMock = null!;
        private CompanyService _service = null!;

        [SetUp]
        public void Setup()
        {
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _ownerRepositoryMock = new Mock<IOwnerRepository>();
            _service = new CompanyService(_companyRepositoryMock.Object, _ownerRepositoryMock.Object);
        }

        [Test]
        public async Task CreateCompanyAsync_ShouldSetIsCompanyContactTrue()
        {
            // Arrange
            var createDto = new CreateCompanyDto
            {
                OwnerId = 1,
                CompanyName = "Acme Corp",
                CompanySite = "https://acme.com"
            };

            var owner = new Owner
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Phone = "+1234567890",
                IsCompanyContact = false
            };

            var createdCompany = new Company
            {
                Id = 1,
                OwnerId = 1,
                CompanyName = "Acme Corp",
                CompanySite = "https://acme.com",
                Owner = owner
            };

            _ownerRepositoryMock.Setup(r => r.ExistsAsync(1))
                .ReturnsAsync(true);

            _ownerRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(owner);

            _companyRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Company>()))
                .ReturnsAsync((Company c) => { c.Id = 1; return c; });

            _companyRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(createdCompany);

            // Act
            var result = await _service.CreateCompanyAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.CompanyName.Should().Be("Acme Corp");
            _ownerRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Owner>(o => o.IsCompanyContact == true)), Times.Once);
        }

        [Test]
        public void CreateCompanyAsync_ShouldThrowException_WhenOwnerNotFound()
        {
            // Arrange
            var createDto = new CreateCompanyDto
            {
                OwnerId = 999,
                CompanyName = "Acme Corp",
                CompanySite = "https://acme.com"
            };

            _ownerRepositoryMock.Setup(r => r.ExistsAsync(999))
                .ReturnsAsync(false);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _service.CreateCompanyAsync(createDto));
        }

        [Test]
        public async Task GetCompanyByIdAsync_ShouldReturnDto_WhenExists()
        {
            // Arrange
            var company = new Company
            {
                Id = 1,
                OwnerId = 1,
                CompanyName = "Tech Inc",
                CompanySite = "https://tech.com",
                Owner = new Owner 
                { 
                    Id = 1, 
                    FirstName = "Jane", 
                    LastName = "Smith",
                    Email = "jane@example.com",
                    Phone = "+0987654321"
                }
            };

            _companyRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(company);

            // Act
            var result = await _service.GetCompanyByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.CompanyName.Should().Be("Tech Inc");
            result.Owner.Should().NotBeNull();
            result.Owner!.FirstName.Should().Be("Jane");
        }

        [Test]
        public async Task UpdateCompanyAsync_ShouldReturnUpdatedDto()
        {
            // Arrange
            var existingCompany = new Company
            {
                Id = 1,
                OwnerId = 1,
                CompanyName = "Original Name",
                CompanySite = "https://original.com",
                Owner = new Owner 
                { 
                    Id = 1, 
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Email = "bob@example.com",
                    Phone = "+1111111111"
                }
            };

            var updateDto = new UpdateCompanyDto
            {
                CompanyName = "Updated Name"
            };

            _companyRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(existingCompany);

            _companyRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Company>()))
                .ReturnsAsync((Company c) => c);

            // Act
            var result = await _service.UpdateCompanyAsync(1, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.CompanyName.Should().Be("Updated Name");
        }

        [Test]
        public async Task DeleteCompanyAsync_ShouldReturnTrue_WhenSuccessful()
        {
            // Arrange
            _companyRepositoryMock.Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteCompanyAsync(1);

            // Assert
            result.Should().BeTrue();
        }
    }
}
