using NUnit.Framework;
using FluentAssertions;
using BackendApi.Models.DTOs;
using BackendApi.Validators;

namespace BackendApi.Tests.Validators
{
    [TestFixture]
    public class CreateCompanyValidatorTests
    {
        private CreateCompanyValidator _validator = null!;

        [SetUp]
        public void Setup()
        {
            _validator = new CreateCompanyValidator();
        }

        [Test]
        public void Validate_ShouldPass_WhenAllFieldsValid()
        {
            // Arrange
            var dto = new CreateCompanyDto
            {
                OwnerId = 1,
                CompanyName = "Acme Corp",
                CompanySite = "https://acme.com"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void Validate_ShouldFail_WhenOwnerIdZero()
        {
            // Arrange
            var dto = new CreateCompanyDto
            {
                OwnerId = 0,
                CompanyName = "Acme Corp",
                CompanySite = "https://acme.com"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "OwnerId");
        }

        [Test]
        public void Validate_ShouldFail_WhenCompanyNameEmpty()
        {
            // Arrange
            var dto = new CreateCompanyDto
            {
                OwnerId = 1,
                CompanyName = "",
                CompanySite = "https://acme.com"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CompanyName");
        }

        [Test]
        public void Validate_ShouldFail_WhenCompanySiteInvalidUrl()
        {
            // Arrange
            var dto = new CreateCompanyDto
            {
                OwnerId = 1,
                CompanyName = "Acme Corp",
                CompanySite = "not-a-url"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CompanySite" && e.ErrorMessage.Contains("URL"));
        }

        [Test]
        public void Validate_ShouldPass_WhenCompanySiteEmpty()
        {
            // Arrange
            var dto = new CreateCompanyDto
            {
                OwnerId = 1,
                CompanyName = "Acme Corp",
                CompanySite = ""
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void Validate_ShouldPass_WhenCompanySiteHttps()
        {
            // Arrange
            var dto = new CreateCompanyDto
            {
                OwnerId = 1,
                CompanyName = "Acme Corp",
                CompanySite = "https://example.com"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void Validate_ShouldPass_WhenCompanySiteHttp()
        {
            // Arrange
            var dto = new CreateCompanyDto
            {
                OwnerId = 1,
                CompanyName = "Acme Corp",
                CompanySite = "http://example.com"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
