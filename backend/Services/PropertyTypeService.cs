using BackendApi.Interfaces;
using BackendApi.Models;
using BackendApi.Models.DTOs;

namespace BackendApi.Services
{
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;

        public PropertyTypeService(IPropertyTypeRepository propertyTypeRepository)
        {
            _propertyTypeRepository = propertyTypeRepository;
        }

        public async Task<IEnumerable<PropertyTypeDto>> GetAllPropertyTypesAsync()
        {
            var propertyTypes = await _propertyTypeRepository.GetAllAsync();
            return propertyTypes.Select(MapToDto);
        }

        private static PropertyTypeDto MapToDto(PropertyType propertyType)
        {
            return new PropertyTypeDto
            {
                Id = propertyType.Id,
                Type = propertyType.Type
            };
        }
    }
}
