namespace PROG7311.API.Patterns.States
{
    public class ActiveState : IContractState
    {
        public bool CanCreateServiceRequest() => true;
        public bool CanActivate() => false;
        public bool CanExpire() => true;
        public bool CanPutOnHold() => true;
        public string GetStateName() => "Active";
    }
}
