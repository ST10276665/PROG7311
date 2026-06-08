using Microsoft.Extensions.Logging;

namespace PROG7311.API.Patterns.Observers
{
    public class ManagerNotifier : IContractObserver
    {
        private readonly ILogger<ManagerNotifier> _logger;

        public ManagerNotifier(ILogger<ManagerNotifier> logger)
        {
            _logger = logger;
        }

        public void OnContractStatusChanged(int contractId, string oldStatus, string newStatus)
        {
            _logger.LogInformation("Contract {ContractId} status changed from {Old} to {New}", contractId, oldStatus, newStatus);
        }
    }
}
