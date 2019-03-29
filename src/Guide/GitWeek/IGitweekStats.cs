using System.Collections.Generic;

namespace Guide.GitWeek
{
    public interface IGitweekStats
    {
        IEnumerable<GitweekParticipant> GetParticipants();
        string[] GetLeaderboards();
    }
}