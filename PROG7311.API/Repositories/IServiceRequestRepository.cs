using System.Collections.Generic;
using System.Threading.Tasks;
using PROG7311.API.Models;

namespace PROG7311.API.Repositories
{
    public interface IServiceRequestRepository
    {
        Task<IEnumerable<ServiceRequest>> GetAllAsync();
        Task<ServiceRequest?> GetByIdAsync(int id);
        Task<ServiceRequest> CreateAsync(ServiceRequest request);
        Task<ServiceRequest?> UpdateAsync(int id, ServiceRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
