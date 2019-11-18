using System.Threading.Tasks;

namespace Guide.Core.Boundaries.ToggleHelperRole
{
    public interface IToggleHelperRole
    {
        Task Execute(ToggleHelperRoleInput input);
    }
}
