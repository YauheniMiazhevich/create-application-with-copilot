using BackendApi.Interfaces;
using BackendApi.Models;
using BackendApi.Models.DTOs;

namespace BackendApi.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IPropertyRepository _propertyRepository;

        public OwnerService(
            IOwnerRepository ownerRepository,
            ICompanyRepository companyRepository,
            IPropertyRepository propertyRepository)
        {
            _ownerRepository = ownerRepository;
            _companyRepository = companyRepository;
            _propertyRepository = propertyRepository;
        }

        public async Task<IEnumerable<OwnerDto>> GetAllOwnersAsync()
        {
            var owners = await _ownerRepository.GetAllAsync();
            return owners.Select(MapToDto);
        }

        public async Task<OwnerDto?> GetOwnerByIdAsync(int id)
        {
            var owner = await _ownerRepository.GetByIdAsync(id);
            return owner == null ? null : MapToDto(owner);
        }

        public async Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createOwnerDto)
        {
            var owner = new Owner
            {
                FirstName = createOwnerDto.FirstName,
                LastName = createOwnerDto.LastName,
                Email = createOwnerDto.Email,
                Phone = createOwnerDto.Phone,
                Address = createOwnerDto.Address,
                Description = createOwnerDto.Description,
                IsCompanyContact = false
            };

            var createdOwner = await _ownerRepository.CreateAsync(owner);
            return MapToDto(createdOwner);
        }

        public async Task<OwnerDto?> UpdateOwnerAsync(int id, UpdateOwnerDto updateOwnerDto)
        {
            var owner = await _ownerRepository.GetByIdAsync(id);
            if (owner == null)
                return null;

            if (!string.IsNullOrEmpty(updateOwnerDto.FirstName))
                owner.FirstName = updateOwnerDto.FirstName;
            
            if (!string.IsNullOrEmpty(updateOwnerDto.LastName))
                owner.LastName = updateOwnerDto.LastName;
            
            if (!string.IsNullOrEmpty(updateOwnerDto.Email))
                owner.Email = updateOwnerDto.Email;
            
            if (!string.IsNullOrEmpty(updateOwnerDto.Phone))
                owner.Phone = updateOwnerDto.Phone;
            
            if (updateOwnerDto.Address != null)
                owner.Address = updateOwnerDto.Address;
            
            if (updateOwnerDto.Description != null)
                owner.Description = updateOwnerDto.Description;

            var updatedOwner = await _ownerRepository.UpdateAsync(owner);
            return MapToDto(updatedOwner);
        }

        public async Task<bool> DeleteOwnerAsync(int id)
        {
            // Check if owner has associated companies
            var companies = await _companyRepository.GetByOwnerIdAsync(id);
            if (companies.Any())
                return false;

            // Check if owner has associated properties
            var properties = await _propertyRepository.GetByOwnerIdAsync(id);
            if (properties.Any())
                return false;

            return await _ownerRepository.DeleteAsync(id);
        }

        private static OwnerDto MapToDto(Owner owner)
        {
            return new OwnerDto
            {
                Id = owner.Id,
                FirstName = owner.FirstName,
                LastName = owner.LastName,
                Email = owner.Email,
                Phone = owner.Phone,
                Address = owner.Address,
                Description = owner.Description,
                IsCompanyContact = owner.IsCompanyContact
            };
        }
    }
}
