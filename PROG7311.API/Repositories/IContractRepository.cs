using PROG7311.API.Models;

namespace PROG7311.API.Repositories
{
    public interface IContractRepository
    {
        Task<IEnumerable<Contract>> GetAllAsync(string? status, DateTime? startDate, DateTime? endDate);
        Task<Contract?> GetByIdAsync(int id);
        Task<Contract> CreateAsync(Contract contract);
        Task<Contract?> UpdateStatusAsync(int id, string newStatus);
        Task<Contract?> UpdateAsync(int id, Contract contract);
        Task<bool> DeleteAsync(int id);
    }
}