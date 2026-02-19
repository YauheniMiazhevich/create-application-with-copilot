using BackendApi.Models.DTOs;
using FluentValidation;

namespace BackendApi.Validators
{
    public class UpdatePropertyValidator : AbstractValidator<UpdatePropertyDto>
    {
        public UpdatePropertyValidator()
        {
            RuleFor(x => x.OwnerId)
                .GreaterThan(0).WithMessage("Owner ID must be greater than 0")
                .When(x => x.OwnerId.HasValue);

            RuleFor(x => x.PropertyTypeId)
                .GreaterThan(0).WithMessage("Property Type ID must be greater than 0")
                .When(x => x.PropertyTypeId.HasValue);

            RuleFor(x => x.PropertyLength)
                .GreaterThan(0).WithMessage("Property length must be greater than 0")
                .When(x => x.PropertyLength.HasValue);

            RuleFor(x => x.PropertyCost)
                .GreaterThan(0).WithMessage("Property cost must be greater than 0")
                .When(x => x.PropertyCost.HasValue);

            RuleFor(x => x.DateOfBuilding)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date of building cannot be in the future")
                .When(x => x.DateOfBuilding.HasValue);

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Country)
                .MaximumLength(100).WithMessage("Country must not exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Country));

            RuleFor(x => x.City)
                .MaximumLength(100).WithMessage("City must not exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.City));

            RuleFor(x => x.Street)
                .MaximumLength(200).WithMessage("Street must not exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Street));

            RuleFor(x => x.ZipCode)
                .MaximumLength(20).WithMessage("Zip code must not exceed 20 characters")
                .When(x => !string.IsNullOrEmpty(x.ZipCode));
        }
    }
}
