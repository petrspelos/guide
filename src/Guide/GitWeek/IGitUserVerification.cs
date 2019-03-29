using System.Collections.Generic;
using System.Threading.Tasks;

namespace Guide.GitWeek
{
    public interface IGitUserVerification
    {
        Task<string> GetVerificationStringFor(ulong userId, string githubUsername);
        Task<bool> Verify(ulong userId, string githubUsername);
        Dictionary<ulong, string> GetVerifiedUsers();
    }
}