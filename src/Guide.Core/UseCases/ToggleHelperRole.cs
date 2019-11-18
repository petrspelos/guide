using System.Threading.Tasks;
using Guide.Core.Boundaries.ToggleHelperRole;
using Guide.Core.Services;

namespace Guide.Core.UseCases
{
    public sealed class ToggleHelperRole : IToggleHelperRole
    {
        private readonly IToggleHelperRoleOutputPort _output;
        private readonly IRoleService _roles;

        public ToggleHelperRole(IToggleHelperRoleOutputPort output, IRoleService roles)
        {
            _output = output;
            _roles = roles;
        }

        public async Task Execute(ToggleHelperRoleInput input)
        {
            if(await _roles.UserIsHelper(input.UserId))
            {
                await _roles.UnassignHelperRole(input.UserId);
                _output.Default(new ToggleHelperRoleOutput(false));
            }
            else
            {
                await _roles.AssignHelperRole(input.UserId);
                _output.Default(new ToggleHelperRoleOutput(true));
            }
        }
    }
}
