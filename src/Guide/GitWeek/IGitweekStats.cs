using System;
using System.Collections.Generic;

namespace Guide.GitWeek
{
    public interface IGitweekStats
    {
        DateTime WarmupStart { get; set; }
        DateTime GitweekStart { get; set; }
        IEnumerable<GitweekParticipant> GetParticipants();
        string[] GetLeaderboards();
        string GetStats(GitweekParticipant user);
    }
}