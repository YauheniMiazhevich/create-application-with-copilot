using BackendApi.Models;

namespace BackendApi.Interfaces
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetAllAsync();
        Task<Owner?> GetByIdAsync(int id);
        Task<Owner> CreateAsync(Owner owner);
        Task<Owner> UpdateAsync(Owner owner);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
