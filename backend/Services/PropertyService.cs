using BackendApi.Interfaces;
using BackendApi.Models;
using BackendApi.Models.DTOs;

namespace BackendApi.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public PropertyService(
            IPropertyRepository propertyRepository,
            IOwnerRepository ownerRepository,
            IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyRepository = propertyRepository;
            _ownerRepository = ownerRepository;
            _propertyTypeRepository = propertyTypeRepository;
        }

        public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
        {
            var properties = await _propertyRepository.GetAllAsync();
            return properties.Select(MapToDto);
        }

        public async Task<PropertyDto?> GetPropertyByIdAsync(int id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            return property == null ? null : MapToDto(property);
        }

        public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createPropertyDto)
        {
            // Validate that owner exists
            var ownerExists = await _ownerRepository.ExistsAsync(createPropertyDto.OwnerId);
            if (!ownerExists)
                throw new ArgumentException($"Owner with ID {createPropertyDto.OwnerId} not found");

            // Validate that property type exists
            var propertyTypeExists = await _propertyTypeRepository.ExistsAsync(createPropertyDto.PropertyTypeId);
            if (!propertyTypeExists)
                throw new ArgumentException($"Property Type with ID {createPropertyDto.PropertyTypeId} not found");

            var property = new Property
            {
                OwnerId = createPropertyDto.OwnerId,
                PropertyTypeId = createPropertyDto.PropertyTypeId,
                PropertyLength = createPropertyDto.PropertyLength,
                PropertyCost = createPropertyDto.PropertyCost,
                DateOfBuilding = DateTime.SpecifyKind(createPropertyDto.DateOfBuilding, DateTimeKind.Utc),
                Description = createPropertyDto.Description,
                Country = createPropertyDto.Country,
                City = createPropertyDto.City,
                Street = createPropertyDto.Street,
                ZipCode = createPropertyDto.ZipCode
            };

            var createdProperty = await _propertyRepository.CreateAsync(property);
            
            // Reload to get navigation properties
            var propertyWithNav = await _propertyRepository.GetByIdAsync(createdProperty.Id);
            return MapToDto(propertyWithNav!);
        }

        public async Task<PropertyDto?> UpdatePropertyAsync(int id, UpdatePropertyDto updatePropertyDto)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            if (property == null)
                return null;

            // Validate that owner exists if it's being updated
            if (updatePropertyDto.OwnerId.HasValue)
            {
                var ownerExists = await _ownerRepository.ExistsAsync(updatePropertyDto.OwnerId.Value);
                if (!ownerExists)
                    throw new ArgumentException($"Owner with ID {updatePropertyDto.OwnerId.Value} not found");

                property.OwnerId = updatePropertyDto.OwnerId.Value;
            }

            // Validate property type if it's being updated
            if (updatePropertyDto.PropertyTypeId.HasValue)
            {
                var propertyTypeExists = await _propertyTypeRepository.ExistsAsync(updatePropertyDto.PropertyTypeId.Value);
                if (!propertyTypeExists)
                    throw new ArgumentException($"Property Type with ID {updatePropertyDto.PropertyTypeId} not found");
                
                property.PropertyTypeId = updatePropertyDto.PropertyTypeId.Value;
            }

            if (updatePropertyDto.PropertyLength.HasValue)
                property.PropertyLength = updatePropertyDto.PropertyLength.Value;
            
            if (updatePropertyDto.PropertyCost.HasValue)
                property.PropertyCost = updatePropertyDto.PropertyCost.Value;
            
            if (updatePropertyDto.DateOfBuilding.HasValue)
                property.DateOfBuilding = DateTime.SpecifyKind(updatePropertyDto.DateOfBuilding.Value, DateTimeKind.Utc);
            
            if (updatePropertyDto.Description != null)
                property.Description = updatePropertyDto.Description;
            
            if (!string.IsNullOrEmpty(updatePropertyDto.Country))
                property.Country = updatePropertyDto.Country;
            
            if (!string.IsNullOrEmpty(updatePropertyDto.City))
                property.City = updatePropertyDto.City;
            
            if (updatePropertyDto.Street != null)
                property.Street = updatePropertyDto.Street;
            
            if (updatePropertyDto.ZipCode != null)
                property.ZipCode = updatePropertyDto.ZipCode;

            await _propertyRepository.UpdateAsync(property);
            
            // Reload to get navigation properties
            var propertyWithNav = await _propertyRepository.GetByIdAsync(id);
            return MapToDto(propertyWithNav!);
        }

        public async Task<bool> DeletePropertyAsync(int id)
        {
            return await _propertyRepository.DeleteAsync(id);
        }

        private static PropertyDto MapToDto(Property property)
        {
            return new PropertyDto
            {
                Id = property.Id,
                OwnerId = property.OwnerId,
                PropertyTypeId = property.PropertyTypeId,
                PropertyLength = property.PropertyLength,
                PropertyCost = property.PropertyCost,
                DateOfBuilding = property.DateOfBuilding,
                Description = property.Description,
                Country = property.Country,
                City = property.City,
                Street = property.Street,
                ZipCode = property.ZipCode,
                Owner = property.Owner == null ? null : new OwnerDto
                {
                    Id = property.Owner.Id,
                    FirstName = property.Owner.FirstName,
                    LastName = property.Owner.LastName,
                    Email = property.Owner.Email,
                    Phone = property.Owner.Phone,
                    Address = property.Owner.Address,
                    Description = property.Owner.Description,
                    IsCompanyContact = property.Owner.IsCompanyContact
                },
                PropertyType = property.PropertyType == null ? null : new PropertyTypeDto
                {
                    Id = property.PropertyType.Id,
                    Type = property.PropertyType.Type
                }
            };
        }
    }
}
