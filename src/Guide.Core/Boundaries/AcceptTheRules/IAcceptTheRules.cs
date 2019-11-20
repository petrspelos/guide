using System.Threading.Tasks;

namespace Guide.Core.Boundaries.AcceptTheRules
{
    public interface IAcceptTheRules
    {
        IAcceptTheRulesOutputPort Output { get; set; }
        Task Execute(AcceptTheRulesInput input);
    }
}
