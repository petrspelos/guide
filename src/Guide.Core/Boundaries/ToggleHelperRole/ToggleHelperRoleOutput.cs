namespace Guide.Core.Boundaries.ToggleHelperRole
{
    public sealed class ToggleHelperRoleOutput
    {
        public bool RoleAdded { get; private set; }

        public ToggleHelperRoleOutput(bool roleAdded)
        {
            RoleAdded = roleAdded;
        }
    }
}
