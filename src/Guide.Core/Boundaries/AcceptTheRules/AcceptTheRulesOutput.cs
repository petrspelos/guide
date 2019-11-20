namespace Guide.Core.Boundaries.AcceptTheRules
{
    public sealed class AcceptTheRulesOutput
    {
        public string AcceptedName { get; private set; }
        public string Bio { get; private set; }

        public AcceptTheRulesOutput(string acceptedName, string bio)
        {
            AcceptedName = acceptedName;
            Bio = bio;
        }
    }
}
