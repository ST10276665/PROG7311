using PROG7311.Models;

namespace PROG7311.Patterns.Strategies
{
    public class CostValidation : IValidationStrategy
    {
        public string ErrorMessage => "Cost must be greater than zero.";

        public bool Validate(ServiceRequest request, Contract contract)
        {
            return request.Cost > 0;
        }
    }
}
