using BackendApi.Models.DTOs;

namespace BackendApi.Interfaces
{
    public interface IOwnerService
    {
        Task<IEnumerable<OwnerDto>> GetAllOwnersAsync();
        Task<OwnerDto?> GetOwnerByIdAsync(int id);
        Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createOwnerDto);
        Task<OwnerDto?> UpdateOwnerAsync(int id, UpdateOwnerDto updateOwnerDto);
        Task<bool> DeleteOwnerAsync(int id);
    }
}
