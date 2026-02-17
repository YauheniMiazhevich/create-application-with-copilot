using BackendApi.Data;
using BackendApi.Interfaces;
using BackendApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Repositories
{
    public class PropertyTypeRepository : IPropertyTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public PropertyTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PropertyType>> GetAllAsync()
        {
            return await _context.PropertyTypes.ToListAsync();
        }

        public async Task<PropertyType?> GetByIdAsync(int id)
        {
            return await _context.PropertyTypes.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PropertyTypes.AnyAsync(pt => pt.Id == id);
        }
    }
}
