using NUnit.Framework;
using FluentAssertions;
using BackendApi.Models.DTOs;
using BackendApi.Validators;

namespace BackendApi.Tests.Validators
{
    [TestFixture]
    public class CreateOwnerValidatorTests
    {
        private CreateOwnerValidator _validator = null!;

        [SetUp]
        public void Setup()
        {
            _validator = new CreateOwnerValidator();
        }

        [Test]
        public void Validate_ShouldPass_WhenAllFieldsValid()
        {
            // Arrange
            var dto = new CreateOwnerDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "+1234567890",
                Address = "123 Main St",
                Description = "Test owner"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void Validate_ShouldFail_WhenFirstNameEmpty()
        {
            // Arrange
            var dto = new CreateOwnerDto
            {
                FirstName = "",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "+1234567890"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
        }

        [Test]
        public void Validate_ShouldFail_WhenEmailInvalid()
        {
            // Arrange
            var dto = new CreateOwnerDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "invalid-email",
                Phone = "+1234567890"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email" && e.ErrorMessage.Contains("email"));
        }

        [Test]
        public void Validate_ShouldFail_WhenPhoneInvalidFormat()
        {
            // Arrange
            var dto = new CreateOwnerDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "invalid@phone#"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Phone");
        }

        [Test]
        public void Validate_ShouldFail_WhenFirstNameTooLong()
        {
            // Arrange
            var dto = new CreateOwnerDto
            {
                FirstName = new string('A', 101),
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "+1234567890"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "FirstName" && e.ErrorMessage.Contains("100"));
        }

        [Test]
        public void Validate_ShouldPass_WhenOptionalFieldsEmpty()
        {
            // Arrange
            var dto = new CreateOwnerDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Phone = "+1234567890",
                Address = "",
                Description = ""
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
