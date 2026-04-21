using PROG7311.Models;

namespace PROG7311.Patterns.Strategies
{
    public interface IValidationStrategy
    {
        bool Validate(ServiceRequest request, Contract contract);
        string ErrorMessage { get; }
    }
}
