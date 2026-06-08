using PROG7311.API.Models;

namespace PROG7311.API.Repositories
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(int id);
        Task<Client> CreateAsync(Client client);
        Task<Client?> UpdateAsync(int id, Client client);
        Task<bool> DeleteAsync(int id);
    }
}
