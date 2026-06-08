namespace PROG7311.API.Patterns.States
{
    public class ExpiredState : IContractState
    {
        public bool CanCreateServiceRequest() => false;
        public bool CanActivate() => false;
        public bool CanExpire() => false;
        public bool CanPutOnHold() => false;
        public string GetStateName() => "Expired";
    }
}