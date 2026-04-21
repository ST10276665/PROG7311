namespace PROG7311.Patterns.States
{
    public class DraftState : IContractState
    {
        public bool CanCreateServiceRequest() => false;
        public bool CanActivate() => true;
        public bool CanExpire() => false;
        public bool CanPutOnHold() => false;
        public string GetStateName() => "Draft";
    }
}
