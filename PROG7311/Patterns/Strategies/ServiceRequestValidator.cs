using System.Collections.Generic;
using PROG7311.Models;

namespace PROG7311.Patterns.Strategies
{
    public class ServiceRequestValidator
    {
        private readonly List<IValidationStrategy> _strategies;

        public ServiceRequestValidator()
        {
            _strategies = new List<IValidationStrategy>
            {
                new ActiveContractValidation(),
                new CostValidation()
            };
        }

        public (bool IsValid, string ErrorMessage) Validate(ServiceRequest request, Contract contract)
        {
            foreach (var s in _strategies)
            {
                if (!s.Validate(request, contract))
                {
                    return (false, s.ErrorMessage);
                }
            }

            return (true, string.Empty);
        }
    }
}
