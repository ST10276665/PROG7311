namespace PROG7311.Patterns.Observers
{
    public interface IContractObserver
    {
        void OnContractStatusChanged(int contractId, string oldStatus, string newStatus);
    }
}
