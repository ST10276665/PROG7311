using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PROG7311.API.Data;
using PROG7311.API.Models;

namespace PROG7311.API.Repositories
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public ServiceRequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ServiceRequest>> GetAllAsync()
        {
            return await _db.ServiceRequests
                .Include(s => s.Contract)
                    .ThenInclude(c => c.Client)
                .ToListAsync();
        }

        public async Task<ServiceRequest?> GetByIdAsync(int id)
        {
            return await _db.ServiceRequests
                .Include(s => s.Contract)
                    .ThenInclude(c => c.Client)
                .FirstOrDefaultAsync(s => s.ServiceRequestId == id);
        }

        public async Task<ServiceRequest> CreateAsync(ServiceRequest request)
        {
            _db.ServiceRequests.Add(request);
            await _db.SaveChangesAsync();
            return request;
        }

        public async Task<ServiceRequest?> UpdateAsync(int id, ServiceRequest request)
        {
            var existing = await _db.ServiceRequests.FindAsync(id);
            if (existing == null) return null;

            existing.Description = request.Description;
            existing.Cost = request.Cost;
            existing.Status = request.Status;

            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.ServiceRequests.FindAsync(id);
            if (existing == null) return false;

            _db.ServiceRequests.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
