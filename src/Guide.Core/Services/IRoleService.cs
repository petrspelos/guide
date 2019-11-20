using System.Threading.Tasks;

namespace Guide.Core.Services
{
    public interface IRoleService
    {
        Task<bool> UserIsHelper(ulong id);
        Task UnassignHelperRole(ulong id);
        Task AssignHelperRole(ulong id);
        Task AssignMemberRole(ulong id);
    }
}
