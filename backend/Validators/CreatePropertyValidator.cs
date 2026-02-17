using BackendApi.Models.DTOs;
using FluentValidation;

namespace BackendApi.Validators
{
    public class CreatePropertyValidator : AbstractValidator<CreatePropertyDto>
    {
        public CreatePropertyValidator()
        {
            RuleFor(x => x.OwnerId)
                .GreaterThan(0).WithMessage("Owner ID must be greater than 0");

            RuleFor(x => x.PropertyTypeId)
                .GreaterThan(0).WithMessage("Property Type ID must be greater than 0");

            RuleFor(x => x.PropertyLength)
                .GreaterThan(0).WithMessage("Property length must be greater than 0");

            RuleFor(x => x.PropertyCost)
                .GreaterThan(0).WithMessage("Property cost must be greater than 0");

            RuleFor(x => x.DateOfBuilding)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date of building cannot be in the future");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(100).WithMessage("Country must not exceed 100 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100).WithMessage("City must not exceed 100 characters");

            RuleFor(x => x.Street)
                .MaximumLength(200).WithMessage("Street must not exceed 200 characters");

            RuleFor(x => x.ZipCode)
                .MaximumLength(20).WithMessage("Zip code must not exceed 20 characters");
        }
    }
}
