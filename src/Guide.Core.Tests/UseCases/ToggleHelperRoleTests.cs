using Xunit;
using Moq;
using Guide.Core.Boundaries.ToggleHelperRole;
using Guide.Core.Services;
using Guide.Core.UseCases;
using System.Threading.Tasks;

namespace Guide.Core.Tests
{
    public class ToggleHelperRoleTests
    {
        private readonly Mock<IToggleHelperRoleOutputPort> _output;
        private readonly Mock<IRoleService> _role;
        private readonly IToggleHelperRole _useCase;

        public ToggleHelperRoleTests()
        {
            _output = new Mock<IToggleHelperRoleOutputPort>();
            _role = new Mock<IRoleService>();
            _useCase = new ToggleHelperRole(_output.Object, _role.Object);
        }

        [Fact]
        public async Task NonHelper_ShouldGetHelperRole()
        {
            const ulong userId = 873589715;

            SetUserIsHelper(userId, false);

            await _useCase.Execute(new ToggleHelperRoleInput(userId));

            AssertHelperAssigned(userId);
        }

        [Fact]
        public async Task Helper_ShouldLoseHelperRole()
        {
            const ulong userId = 1234556789;

            SetUserIsHelper(userId, true);

            await _useCase.Execute(new ToggleHelperRoleInput(userId));

            AssertHelperUnassigned(userId);
        }

        private void SetUserIsHelper(ulong id, bool isHelper)
            => _role.Setup(r => r.UserIsHelper(It.Is<ulong>(i => i == id)))
                .Returns(Task.FromResult(isHelper));

        private void AssertHelperAssigned(ulong id)
        {
            _role.Verify(r => r.AssignHelperRole(It.Is<ulong>(i => i == id)), Times.Once);
            _output.Verify(o => o.Default(It.Is<ToggleHelperRoleOutput>(x => x.RoleAdded == true)), Times.Once);
        }

        private void AssertHelperUnassigned(ulong id)
        {
            _role.Verify(r => r.UnassignHelperRole(It.Is<ulong>(i => i == id)), Times.Once);
            _output.Verify(o => o.Default(It.Is<ToggleHelperRoleOutput>(x => x.RoleAdded == false)), Times.Once);
        }
    }
}
