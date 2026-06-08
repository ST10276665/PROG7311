namespace PROG7311.API.Patterns.States
{
    public class OnHoldState : IContractState
    {
        public bool CanCreateServiceRequest() => false;
        public bool CanActivate() => true;
        public bool CanExpire() => true;
        public bool CanPutOnHold() => false;
        public string GetStateName() => "On Hold";
    }
}