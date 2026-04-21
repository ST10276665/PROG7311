using PROG7311.Models;
using PROG7311.Patterns.States;
using PROG7311.Patterns.Strategies;
using Xunit;

namespace PROG7311.Tests
{
    public class WorkflowValidationTests
    {
        private readonly ServiceRequestValidator _validator = new();

        [Fact]
        public void ServiceRequest_ActiveContract_IsAllowed()
        {
            var contract = new Contract { Status = "Active" };
            var request = new ServiceRequest { Cost = 100, Description = "Test" };

            var (isValid, _) = _validator.Validate(request, contract);

            Assert.True(isValid);
        }

        [Fact]
        public void ServiceRequest_ExpiredContract_IsBlocked()
        {
            var contract = new Contract { Status = "Expired" };
            var request = new ServiceRequest { Cost = 100, Description = "Test" };

            var (isValid, errorMessage) = _validator.Validate(request, contract);

            Assert.False(isValid);
            Assert.Equal("Service requests can only be created for Active contracts.", errorMessage);
        }

        [Fact]
        public void ServiceRequest_OnHoldContract_IsBlocked()
        {
            var contract = new Contract { Status = "On Hold" };
            var request = new ServiceRequest { Cost = 100, Description = "Test" };

            var (isValid, _) = _validator.Validate(request, contract);

            Assert.False(isValid);
        }

        [Fact]
        public void ServiceRequest_DraftContract_IsBlocked()
        {
            var contract = new Contract { Status = "Draft" };
            var request = new ServiceRequest { Cost = 100, Description = "Test" };

            var (isValid, _) = _validator.Validate(request, contract);

            Assert.False(isValid);
        }

        [Fact]
        public void ServiceRequest_ZeroCost_IsBlocked()
        {
            var contract = new Contract { Status = "Active" };
            var request = new ServiceRequest { Cost = 0, Description = "Test" };

            var (isValid, errorMessage) = _validator.Validate(request, contract);

            Assert.False(isValid);
            Assert.Equal("Cost must be greater than zero.", errorMessage);
        }
    }
}