namespace Guide.Core.Boundaries.AcceptTheRules
{
    public interface IAcceptTheRulesOutputPort : IErrorHandler
    {
        void Default(AcceptTheRulesOutput output);
    }
}
