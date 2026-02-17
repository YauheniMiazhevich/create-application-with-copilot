using BackendApi.Models;

namespace BackendApi.Interfaces
{
    public interface IPropertyTypeRepository
    {
        Task<IEnumerable<PropertyType>> GetAllAsync();
        Task<PropertyType?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
