using System;
using System.Threading.Tasks;
using Guide.Core.Boundaries.AcceptTheRules;

namespace Guide.Core.UseCases
{
    public sealed class AcceptTheRules : IAcceptTheRules
    {
        public Task Execute(AcceptTheRulesInput input)
        {
            if(input is null)
                throw new Exception($"The parameter {nameof(input)} cannot be null.");

                return Task.CompletedTask;
        }
    }
}
