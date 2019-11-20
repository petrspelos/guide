using System;
using System.Threading.Tasks;
using Guide.Core.Boundaries.ToggleHelperRole;
using Guide.Core.Services;

namespace Guide.Core.UseCases
{
    public sealed class ToggleHelperRole : IToggleHelperRole
    {
        public IToggleHelperRoleOutputPort Output { get; set; }
        private readonly IRoleService _roles;

        public ToggleHelperRole(IRoleService roles)
        {
            _roles = roles;
        }

        public async Task Execute(ToggleHelperRoleInput input)
        {
            if(input is null)
                throw new Exception($"The parameter {nameof(input)} cannot be null.");

            if(await _roles.UserIsHelper(input.UserId))
            {
                await _roles.UnassignHelperRole(input.UserId);
                Output.Default(new ToggleHelperRoleOutput(false));
            }
            else
            {
                await _roles.AssignHelperRole(input.UserId);
                Output.Default(new ToggleHelperRoleOutput(true));
            }
        }
    }
}
