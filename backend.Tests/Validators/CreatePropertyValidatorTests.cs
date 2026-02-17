using NUnit.Framework;
using FluentAssertions;
using BackendApi.Models.DTOs;
using BackendApi.Validators;

namespace BackendApi.Tests.Validators
{
    [TestFixture]
    public class CreatePropertyValidatorTests
    {
        private CreatePropertyValidator _validator = null!;

        [SetUp]
        public void Setup()
        {
            _validator = new CreatePropertyValidator();
        }

        [Test]
        public void Validate_ShouldPass_WhenAllFieldsValid()
        {
            // Arrange
            var dto = new CreatePropertyDto
            {
                OwnerId = 1,
                PropertyTypeId = 1,
                PropertyLength = 100.5m,
                PropertyCost = 250000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Description = "Beautiful property",
                Country = "USA",
                City = "New York",
                Street = "5th Avenue",
                ZipCode = "10001"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void Validate_ShouldFail_WhenPropertyLengthZero()
        {
            // Arrange
            var dto = new CreatePropertyDto
            {
                OwnerId = 1,
                PropertyTypeId = 1,
                PropertyLength = 0,
                PropertyCost = 250000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Country = "USA",
                City = "New York"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "PropertyLength");
        }

        [Test]
        public void Validate_ShouldFail_WhenPropertyCostNegative()
        {
            // Arrange
            var dto = new CreatePropertyDto
            {
                OwnerId = 1,
                PropertyTypeId = 1,
                PropertyLength = 100m,
                PropertyCost = -1000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Country = "USA",
                City = "New York"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "PropertyCost");
        }

        [Test]
        public void Validate_ShouldFail_WhenDateOfBuildingInFuture()
        {
            // Arrange
            var dto = new CreatePropertyDto
            {
                OwnerId = 1,
                PropertyTypeId = 1,
                PropertyLength = 100m,
                PropertyCost = 250000m,
                DateOfBuilding = DateTime.Now.AddDays(1),
                Country = "USA",
                City = "New York"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "DateOfBuilding" && e.ErrorMessage.Contains("future"));
        }

        [Test]
        public void Validate_ShouldFail_WhenCountryEmpty()
        {
            // Arrange
            var dto = new CreatePropertyDto
            {
                OwnerId = 1,
                PropertyTypeId = 1,
                PropertyLength = 100m,
                PropertyCost = 250000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Country = "",
                City = "New York"
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Country");
        }

        [Test]
        public void Validate_ShouldFail_WhenCityEmpty()
        {
            // Arrange
            var dto = new CreatePropertyDto
            {
                OwnerId = 1,
                PropertyTypeId = 1,
                PropertyLength = 100m,
                PropertyCost = 250000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Country = "USA",
                City = ""
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "City");
        }

        [Test]
        public void Validate_ShouldPass_WhenOptionalFieldsEmpty()
        {
            // Arrange
            var dto = new CreatePropertyDto
            {
                OwnerId = 1,
                PropertyTypeId = 1,
                PropertyLength = 100m,
                PropertyCost = 250000m,
                DateOfBuilding = new DateTime(2020, 1, 1),
                Country = "USA",
                City = "New York",
                Description = "",
                Street = "",
                ZipCode = ""
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
