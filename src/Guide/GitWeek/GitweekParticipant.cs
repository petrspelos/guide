using System;
using System.Collections.Generic;

namespace Guide.GitWeek
{
    public class GitweekParticipant
    {
        public ulong DiscordId { get; set; }
        public string GitHubUsername { get; set; }
        public Dictionary<DateTime, int> CommitsByDate { get; set; }
    }
}