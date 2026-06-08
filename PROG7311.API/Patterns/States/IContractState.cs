namespace PROG7311.API.Patterns.States
{
    public interface IContractState
    {
        bool CanCreateServiceRequest();
        bool CanActivate();
        bool CanExpire();
        bool CanPutOnHold();
        string GetStateName();
    }
}