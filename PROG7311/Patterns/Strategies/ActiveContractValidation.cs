using PROG7311.Models;
using PROG7311.Patterns.States;

namespace PROG7311.Patterns.Strategies
{
    public class ActiveContractValidation : IValidationStrategy
    {
        public string ErrorMessage => "Service requests can only be created for Active contracts.";

        public bool Validate(ServiceRequest request, Contract contract)
        {
            var state = ContractStateFactory.GetState(contract.Status);
            return state.CanCreateServiceRequest();
        }
    }
}
