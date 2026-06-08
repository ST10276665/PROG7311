using PROG7311.API.Models;

namespace PROG7311.API.Patterns.Strategies
{
    public interface IValidationStrategy
    {
        bool Validate(ServiceRequest request, Contract contract);
        string ErrorMessage { get; }
    }
}
