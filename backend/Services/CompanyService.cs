using BackendApi.Interfaces;
using BackendApi.Models;
using BackendApi.Models.DTOs;

namespace BackendApi.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IOwnerRepository _ownerRepository;

        public CompanyService(
            ICompanyRepository companyRepository,
            IOwnerRepository ownerRepository)
        {
            _companyRepository = companyRepository;
            _ownerRepository = ownerRepository;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            return companies.Select(MapToDto);
        }

        public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            return company == null ? null : MapToDto(company);
        }

        public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto)
        {
            // Validate that owner exists
            var ownerExists = await _ownerRepository.ExistsAsync(createCompanyDto.OwnerId);
            if (!ownerExists)
                throw new ArgumentException($"Owner with ID {createCompanyDto.OwnerId} not found");

            var company = new Company
            {
                OwnerId = createCompanyDto.OwnerId,
                CompanyName = createCompanyDto.CompanyName,
                CompanySite = createCompanyDto.CompanySite
            };

            var createdCompany = await _companyRepository.CreateAsync(company);

            // Set IsCompanyContact to true for the owner
            var owner = await _ownerRepository.GetByIdAsync(createCompanyDto.OwnerId);
            if (owner != null)
            {
                owner.IsCompanyContact = true;
                await _ownerRepository.UpdateAsync(owner);
            }

            // Reload to get navigation properties
            var companyWithOwner = await _companyRepository.GetByIdAsync(createdCompany.Id);
            return MapToDto(companyWithOwner!);
        }

        public async Task<CompanyDto?> UpdateCompanyAsync(int id, UpdateCompanyDto updateCompanyDto)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
                return null;

            if (!string.IsNullOrEmpty(updateCompanyDto.CompanyName))
                company.CompanyName = updateCompanyDto.CompanyName;
            
            if (updateCompanyDto.CompanySite != null)
                company.CompanySite = updateCompanyDto.CompanySite;

            var updatedCompany = await _companyRepository.UpdateAsync(company);
            
            // Reload to get navigation properties
            var companyWithOwner = await _companyRepository.GetByIdAsync(id);
            return MapToDto(companyWithOwner!);
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            return await _companyRepository.DeleteAsync(id);
        }

        private static CompanyDto MapToDto(Company company)
        {
            return new CompanyDto
            {
                Id = company.Id,
                OwnerId = company.OwnerId,
                CompanyName = company.CompanyName,
                CompanySite = company.CompanySite,
                Owner = company.Owner == null ? null : new OwnerDto
                {
                    Id = company.Owner.Id,
                    FirstName = company.Owner.FirstName,
                    LastName = company.Owner.LastName,
                    Email = company.Owner.Email,
                    Phone = company.Owner.Phone,
                    Address = company.Owner.Address,
                    Description = company.Owner.Description,
                    IsCompanyContact = company.Owner.IsCompanyContact
                }
            };
        }
    }
}
