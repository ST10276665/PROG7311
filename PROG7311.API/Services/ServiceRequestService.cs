using System.Threading.Tasks;
using PROG7311.API.Models;
using PROG7311.API.Patterns.Observers;
using PROG7311.API.Patterns.Strategies;
using PROG7311.API.Repositories;

namespace PROG7311.API.Services
{
    public class ServiceRequestService
    {
        private readonly IServiceRequestRepository _repo;
        private readonly IContractRepository _contractRepo;
        private readonly ServiceRequestValidator _validator;
        private readonly ContractNotifier _notifier;

        public ServiceRequestService(
            IServiceRequestRepository repo,
            IContractRepository contractRepo,
            ServiceRequestValidator validator,
            ContractNotifier notifier,
            ManagerNotifier managerNotifier)
        {
            _repo = repo;
            _contractRepo = contractRepo;
            _validator = validator;
            _notifier = notifier;

            _notifier.Subscribe(managerNotifier);
        }

        public async Task<(bool Success, string ErrorMessage, ServiceRequest? Request)> CreateAsync(ServiceRequest request)
        {
            var contract = await _contractRepo.GetByIdAsync(request.ContractId);
            if (contract == null)
                return (false, "Contract not found.", null);

            var (isValid, error) = _validator.Validate(request, contract);
            if (!isValid)
                return (false, error, null);

            var created = await _repo.CreateAsync(request);
            return (true, string.Empty, created);
        }
    }
}
