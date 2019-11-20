using System.Threading.Tasks;

namespace Guide.Core.Boundaries.AcceptTheRules
{
    public interface IAcceptTheRules
    {
        Task Execute(AcceptTheRulesInput input);
    }
}
