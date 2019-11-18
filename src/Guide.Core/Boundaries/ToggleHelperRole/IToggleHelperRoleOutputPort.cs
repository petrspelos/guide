namespace Guide.Core.Boundaries.ToggleHelperRole
{
    public interface IToggleHelperRoleOutputPort : IErrorHandler
    {
        void Default(ToggleHelperRoleOutput output);
    }
}
