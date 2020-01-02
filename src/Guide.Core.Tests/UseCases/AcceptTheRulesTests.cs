using System;
using System.Threading.Tasks;
using Guide.Core.Boundaries.AcceptTheRules;
using Guide.Core.Exceptions;
using Guide.Core.Services;
using Guide.Core.UseCases;
using Moq;
using Xunit;

namespace Guide.Core.Tests.UseCases
{
    public class AcceptTheRulesTests
    {
        private readonly Mock<IAcceptTheRulesOutputPort> _output;
        private readonly Mock<INameValidator> _validator;
        private readonly Mock<IRoleService> _roles;
        private readonly IAcceptTheRules _useCase;

        public AcceptTheRulesTests()
        {
            _output = new Mock<IAcceptTheRulesOutputPort>();
            _validator = new Mock<INameValidator>();
            _roles = new Mock<IRoleService>();
            _useCase = new AcceptTheRules(_validator.Object, _roles.Object)
            {
                Output = _output.Object
            };
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [Trait("UseCase", "AcceptTheRules")]
        public void NullOrEmptyName_ShouldThrow(string name)
        {
            Assert.Throws<InvalidInputParameterException>(() => {
                new AcceptTheRulesInput(0, name, "Bio");
            });
        }

        [Fact]
        [Trait("UseCase", "AcceptTheRules")]
        public async Task NullInput_ShouldThrow()
        {
            await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Bio")]
        [Trait("UseCase", "AcceptTheRules")]
        public async Task InvalidName_ShouldOutputError(string bio)
        {
            const string username = "InvalidUsername";
            SetUsernameValid(username, false);

            await _useCase.Execute(new AcceptTheRulesInput(0, username, bio));

            AssertMemberRoleNotAssigned();
            AssertNotNullOrEmptyErrorOutput();
            AssertNoDefaultOutput();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Bio")]
        [Trait("UseCase", "AcceptTheRules")]
        public async Task ValidName_ShouldOutputDefault(string bio)
        {
            const string username = "Username";
            SetUsernameValid(username, true);

            await _useCase.Execute(new AcceptTheRulesInput(0, username, bio));

            AssertMemberRoleAssigned();
            AssertDefaultWasCalled();
            AssertNoErrorOutput();
        }

        private void SetUsernameValid(string username, bool valid)
            => _validator.Setup(v => v.IsValid(It.Is<string>(s => s == username))).Returns(valid);

        private void AssertNotNullOrEmptyErrorOutput()
            => _output.Verify(o => o.Error(It.Is<string>(s => !string.IsNullOrEmpty(s))), Times.Once);

        private void AssertNoDefaultOutput()
            => _output.Verify(o => o.Default(It.IsAny<AcceptTheRulesOutput>()), Times.Never);

        private void AssertDefaultWasCalled()
            => _output.Verify(o => o.Default(It.IsAny<AcceptTheRulesOutput>()), Times.Once);

        private void AssertNoErrorOutput()
            => _output.Verify(o => o.Error(It.IsAny<string>()), Times.Never);

        private void AssertMemberRoleNotAssigned()
            => _roles.Verify(r => r.AssignMemberRole(It.IsAny<ulong>()), Times.Never);
            
        private void AssertMemberRoleAssigned()
            => _roles.Verify(r => r.AssignMemberRole(It.IsAny<ulong>()), Times.Once);
    }
}
