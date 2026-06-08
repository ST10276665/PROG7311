using Microsoft.EntityFrameworkCore;
using PROG7311.API.Data;
using PROG7311.API.Models;

namespace PROG7311.API.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _context.Clients
                .Include(c => c.Contracts)
                .FirstOrDefaultAsync(c => c.ClientId == id);
        }

        public async Task<Client> CreateAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client?> UpdateAsync(int id, Client client)
        {
            var existing = await _context.Clients.FindAsync(id);
            if (existing == null) return null;

            existing.Name = client.Name;
            existing.ContactDetails = client.ContactDetails;
            existing.Region = client.Region;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}