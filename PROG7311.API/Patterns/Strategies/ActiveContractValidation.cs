using PROG7311.API.Models;
using PROG7311.API.Patterns.States;

namespace PROG7311.API.Patterns.Strategies
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
