namespace Guide.Core.Boundaries.ToggleHelperRole
{
    public sealed class ToggleHelperRoleInput
    {
        public ulong UserId { get; private set; }

        public ToggleHelperRoleInput(ulong userId)
        {
            UserId = userId;
        }
    }
}
