using System;
using System.Threading.Tasks;
using Guide.Core.Boundaries.AcceptTheRules;
using Guide.Core.Services;

namespace Guide.Core.UseCases
{
    public sealed class AcceptTheRules : IAcceptTheRules
    {
        public IAcceptTheRulesOutputPort Output { get; set; }
        private readonly INameValidator _validator;
        private readonly IRoleService _roles;

        public AcceptTheRules(INameValidator validator, IRoleService roles)
        {
            _validator = validator;
            _roles = roles;
        }

        public async Task Execute(AcceptTheRulesInput input)
        {
            if(input is null)
                throw new Exception($"The parameter {nameof(input)} cannot be null.");

            if(!_validator.IsValid(input.Username))
            {
                Output.Error("Invalid Username");
                return;
            }

            await _roles.AssignMemberRole(input.UserId);
            Output.Default(new AcceptTheRulesOutput(input.Username, input.Bio));
        }
    }
}
