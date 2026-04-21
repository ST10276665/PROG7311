namespace PROG7311.Patterns.States
{
    public static class ContractStateFactory
    {
        public static IContractState GetState(string status)
        {
            return status switch
            {
                "Active" => new ActiveState(),
                "Expired" => new ExpiredState(),
                "On Hold" => new OnHoldState(),
                _ => new DraftState(),
            };
        }
    }
}
