using BackendApi.Data;
using BackendApi.Interfaces;
using BackendApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendApi.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly ApplicationDbContext _context;

        public OwnerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Owner>> GetAllAsync()
        {
            return await _context.Owners.ToListAsync();
        }

        public async Task<Owner?> GetByIdAsync(int id)
        {
            return await _context.Owners.FindAsync(id);
        }

        public async Task<Owner> CreateAsync(Owner owner)
        {
            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();
            return owner;
        }

        public async Task<Owner> UpdateAsync(Owner owner)
        {
            _context.Entry(owner).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return owner;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var owner = await _context.Owners.FindAsync(id);
            if (owner == null)
                return false;

            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Owners.AnyAsync(o => o.Id == id);
        }
    }
}
