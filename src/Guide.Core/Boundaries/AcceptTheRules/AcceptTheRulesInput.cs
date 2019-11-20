using Guide.Core.Exceptions;

namespace Guide.Core.Boundaries.AcceptTheRules
{
    public sealed class AcceptTheRulesInput
    {
        public ulong UserId { get; private set; }
        public string Username { get; private set; }
        public string Bio { get; private set; }

        public AcceptTheRulesInput(ulong userId, string username, string bio)
        {
            if (string.IsNullOrEmpty(username))
                throw new InvalidInputParameterException("The username cannot be null or empty.");

            Username = username;
            Bio = bio;
            UserId = userId;
        }
    }
}
