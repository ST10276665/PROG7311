namespace PROG7311.API.Patterns.Observers
{
    public interface IContractObserver
    {
        void OnContractStatusChanged(int contractId, string oldStatus, string newStatus);
    }
}
