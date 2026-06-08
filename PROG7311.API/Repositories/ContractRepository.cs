using Microsoft.EntityFrameworkCore;
using PROG7311.API.Data;
using PROG7311.API.Models;

namespace PROG7311.API.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly ApplicationDbContext _context;

        public ContractRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contract>> GetAllAsync(string? status, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Contracts.Include(c => c.Client).AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);

            if (startDate.HasValue)
                query = query.Where(c => c.StartDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.EndDate <= endDate.Value);

            return await query.ToListAsync();
        }

        public async Task<Contract?> GetByIdAsync(int id)
        {
            return await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .FirstOrDefaultAsync(c => c.ContractId == id);
        }

        public async Task<Contract> CreateAsync(Contract contract)
        {
            _context.Contracts.Add(contract);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ContractRepository.CreateAsync] Error saving contract: {ex}");
                throw;
            }

            return contract;
        }

        public async Task<Contract?> UpdateStatusAsync(int id, string newStatus)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return null;

            contract.Status = newStatus;
            await _context.SaveChangesAsync();
            return contract;
        }

        public async Task<Contract?> UpdateAsync(int id, Contract contract)
        {
            var existing = await _context.Contracts.FindAsync(id);
            if (existing == null) return null;

            existing.ClientId = contract.ClientId;
            existing.StartDate = contract.StartDate;
            existing.EndDate = contract.EndDate;
            existing.Status = contract.Status;
            existing.ServiceLevel = contract.ServiceLevel;
            existing.SignedAgreementPath = contract.SignedAgreementPath;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return false;

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}