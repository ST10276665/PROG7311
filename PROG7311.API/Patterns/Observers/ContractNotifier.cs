using System.Collections.Generic;

namespace PROG7311.API.Patterns.Observers
{
    public class ContractNotifier
    {
        private readonly List<IContractObserver> _observers = new();

        public void Subscribe(IContractObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Unsubscribe(IContractObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(int contractId, string oldStatus, string newStatus)
        {
            foreach (var obs in _observers)
            {
                obs.OnContractStatusChanged(contractId, oldStatus, newStatus);
            }
        }
    }
}
