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
    public class CompaniesControllerTests
    {
        private Mock<ICompanyService> _companyServiceMock = null!;
        private CompaniesController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _companyServiceMock = new Mock<ICompanyService>();
            _controller = new CompaniesController(_companyServiceMock.Object);
        }

        [Test]
        public async Task CreateCompany_ShouldReturn201Created_WhenValid()
        {
            // Arrange
            var createDto = new CreateCompanyDto
            {
                OwnerId = 1,
                CompanyName = "Tech Corp",
                CompanySite = "https://techcorp.com"
            };

            var companyDto = new CompanyDto
            {
                Id = 1,
                OwnerId = 1,
                CompanyName = "Tech Corp",
                CompanySite = "https://techcorp.com",
                Owner = new OwnerDto 
                { 
                    Id = 1, 
                    FirstName = "John", 
                    LastName = "Doe",
                    Email = "john@example.com",
                    Phone = "+1234567890",
                    IsCompanyContact = true
                }
            };

            _companyServiceMock.Setup(s => s.CreateCompanyAsync(createDto))
                .ReturnsAsync(companyDto);

            // Act
            var result = await _controller.CreateCompany(createDto);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(201);
            createdResult.Value.Should().BeEquivalentTo(companyDto);
        }

        [Test]
        public async Task GetCompany_ShouldReturn200OK_WhenExists()
        {
            // Arrange
            var companyDto = new CompanyDto
            {
                Id = 1,
                OwnerId = 1,
                CompanyName = "Real Estate LLC",
                CompanySite = "https://realestate.com",
                Owner = new OwnerDto
                {
                    Id = 1,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane@example.com",
                    Phone = "+0987654321",
                    IsCompanyContact = true
                }
            };

            _companyServiceMock.Setup(s => s.GetCompanyByIdAsync(1))
                .ReturnsAsync(companyDto);

            // Act
            var result = await _controller.GetCompany(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(companyDto);
        }

        [Test]
        public async Task GetCompany_ShouldReturn404NotFound_WhenNotExists()
        {
            // Arrange
            _companyServiceMock.Setup(s => s.GetCompanyByIdAsync(999))
                .ReturnsAsync((CompanyDto?)null);

            // Act
            var result = await _controller.GetCompany(999);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }



        [Test]
        public async Task UpdateCompany_ShouldReturn200OK_WhenValid()
        {
            // Arrange
            var updateDto = new UpdateCompanyDto
            {
                CompanyName = "Updated Corp"
            };

            var updatedCompany = new CompanyDto
            {
                Id = 1,
                OwnerId = 1,
                CompanyName = "Updated Corp",
                CompanySite = "https://original.com"
            };

            _companyServiceMock.Setup(s => s.UpdateCompanyAsync(1, updateDto))
                .ReturnsAsync(updatedCompany);

            // Act
            var result = await _controller.UpdateCompany(1, updateDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(updatedCompany);
        }

        [Test]
        public async Task UpdateCompany_ShouldReturn404NotFound_WhenNotExists()
        {
            // Arrange
            var updateDto = new UpdateCompanyDto { CompanyName = "Updated" };

            _companyServiceMock.Setup(s => s.UpdateCompanyAsync(999, updateDto))
                .ReturnsAsync((CompanyDto?)null);

            // Act
            var result = await _controller.UpdateCompany(999, updateDto);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public async Task DeleteCompany_ShouldReturn204NoContent_WhenSuccessful()
        {
            // Arrange
            _companyServiceMock.Setup(s => s.DeleteCompanyAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCompany(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task DeleteCompany_ShouldReturn404NotFound_WhenNotExists()
        {
            // Arrange
            _companyServiceMock.Setup(s => s.DeleteCompanyAsync(999))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteCompany(999);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
