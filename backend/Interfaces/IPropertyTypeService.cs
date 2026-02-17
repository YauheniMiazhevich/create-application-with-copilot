using BackendApi.Models.DTOs;

namespace BackendApi.Interfaces
{
    public interface IPropertyTypeService
    {
        Task<IEnumerable<PropertyTypeDto>> GetAllPropertyTypesAsync();
    }
}
