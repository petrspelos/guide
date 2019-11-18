using System.Threading.Tasks;

namespace Guide.Core.Boundaries.ToggleHelperRole
{
    public interface IToggleHelperRole
    {
        IToggleHelperRoleOutputPort Output { get; set; }
        Task Execute(ToggleHelperRoleInput input);
    }
}
