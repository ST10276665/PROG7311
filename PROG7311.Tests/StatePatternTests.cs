using PROG7311.Patterns.States;
using Xunit;

namespace PROG7311.Tests
{
    public class StatePatternTests
    {
        [Fact]
        public void ActiveState_CanCreateServiceRequest()
        {
            var state = ContractStateFactory.GetState("Active");
            Assert.True(state.CanCreateServiceRequest());
        }

        [Fact]
        public void ExpiredState_CannotCreateServiceRequest()
        {
            var state = ContractStateFactory.GetState("Expired");
            Assert.False(state.CanCreateServiceRequest());
        }

        [Fact]
        public void OnHoldState_CannotCreateServiceRequest()
        {
            var state = ContractStateFactory.GetState("On Hold");
            Assert.False(state.CanCreateServiceRequest());
        }

        [Fact]
        public void DraftState_CannotCreateServiceRequest()
        {
            var state = ContractStateFactory.GetState("Draft");
            Assert.False(state.CanCreateServiceRequest());
        }

        [Fact]
        public void ActiveState_CanExpire()
        {
            var state = ContractStateFactory.GetState("Active");
            Assert.True(state.CanExpire());
        }

        [Fact]
        public void DraftState_CanActivate()
        {
            var state = ContractStateFactory.GetState("Draft");
            Assert.True(state.CanActivate());
        }
    }
}