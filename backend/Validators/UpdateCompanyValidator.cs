using BackendApi.Models.DTOs;
using FluentValidation;

namespace BackendApi.Validators
{
    public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyDto>
    {
        public UpdateCompanyValidator()
        {
            RuleFor(x => x.CompanyName)
                .MaximumLength(200).WithMessage("Company name must not exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.CompanyName));

            RuleFor(x => x.CompanySite)
                .MaximumLength(500).WithMessage("Company site must not exceed 500 characters")
                .Must(BeAValidUrl).WithMessage("Company site must be a valid URL")
                .When(x => !string.IsNullOrEmpty(x.CompanySite));
        }

        private bool BeAValidUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return true;

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
