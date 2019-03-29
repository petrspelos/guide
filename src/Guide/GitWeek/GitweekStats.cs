using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Octokit;

namespace Guide.GitWeek
{
    public class GitweekStats : IGitweekStats
    {
        private const string FailedDayIcon = ":x:";
        private const string PassedDayIcon = ":white_check_mark:";
        private const string FutureDayIcon = ":black_square_button:";
        private const string NoneDayIcon = ":black_medium_square:";
        
        private readonly DateTime warmupStart = new DateTime(2019, 3, 28);
        private readonly DateTime gitweekStart = new DateTime(2019, 3, 31);
        private readonly Regex commitRegex = new Regex("<rect .*?data-count=\"(\\d+)\" data-date=\"(\\d{4}-\\d{2}-\\d{2})\"\\/>");
        private readonly IGitUserVerification _verification;
        private readonly GitHubClient _github;

        public GitweekStats(IGitUserVerification verification, GitHubClient github)
        {
            _verification = verification;
            _github = github;
        }

        public IEnumerable<GitweekParticipant> GetParticipants()
            => _verification
                .GetVerifiedUsers()
                .Select(p => GetParticipant(p.Key, p.Value));

        public string[] GetLeaderboards()
        {
            var users = GetParticipants();

            return users.OrderByDescending(u => u.CommitsByDate.Sum(c => c.Value)).Select(user => GetStats(user)).ToArray();
        }

        private string GetStats(GitweekParticipant user)
        {
            var sb = new StringBuilder();

            var warmupCommits = user.CommitsByDate.Where(c =>
                c.Key.Date >= warmupStart.Date &&
                c.Key.Date <= gitweekStart.Date)
                .ToArray();

            var gitweekCommits = user.CommitsByDate.Where(c =>
                c.Key.Date > gitweekStart.Date)
                .ToArray();
            
            sb.AppendLine($"__**{user.GitHubUsername}**__");
            sb.AppendLine($"_{warmupCommits.Sum(c => c.Value)} commits during warmup._");
            sb.AppendLine($"_{gitweekCommits.Sum(c => c.Value)} commits during git week._");
            sb.AppendLine(GetWarmupCalendar(warmupCommits));
            sb.AppendLine(GetGitweekCalendar(gitweekCommits));

            return sb.ToString();
        }

        private string GetGitweekCalendar(IEnumerable<KeyValuePair<DateTime, int>> gitweekCommits)
        {
            var sb = new StringBuilder();

            sb.Append(GetIconForDay(gitweekCommits, gitweekStart));
            sb.Append(GetIconForDay(gitweekCommits, gitweekStart.AddDays(1)));
            sb.Append(GetIconForDay(gitweekCommits, gitweekStart.AddDays(2)));
            sb.Append(GetIconForDay(gitweekCommits, gitweekStart.AddDays(3)));
            sb.Append(GetIconForDay(gitweekCommits, gitweekStart.AddDays(4)));
            sb.Append(GetIconForDay(gitweekCommits, gitweekStart.AddDays(5)));
            sb.Append(GetIconForDay(gitweekCommits, gitweekStart.AddDays(6)));
            
            return sb.ToString();
        }

        private string GetWarmupCalendar(IEnumerable<KeyValuePair<DateTime, int>> warmupCommits)
        {
            var sb = new StringBuilder();

            sb.Append(NoneDayIcon);
            sb.Append(NoneDayIcon);
            sb.Append(NoneDayIcon);
            sb.Append(GetIconForDay(warmupCommits, warmupStart));
            sb.Append(GetIconForDay(warmupCommits, warmupStart.AddDays(1)));
            sb.Append(GetIconForDay(warmupCommits, warmupStart.AddDays(2)));
            sb.Append(GetIconForDay(warmupCommits, warmupStart.AddDays(3)));
            
            return sb.ToString();
        }

        private string GetIconForDay(IEnumerable<KeyValuePair<DateTime, int>> warmupCommits, DateTime targetDate)
        {
            if (DateTime.Now < targetDate) return FutureDayIcon;
            
            if(warmupCommits.All(p => p.Key.Date != targetDate.Date)) return FailedDayIcon;

            var item = warmupCommits.First(p => p.Key.Date == targetDate.Date).Value;

            return item > 0 ? PassedDayIcon : FailedDayIcon;
        }

        private GitweekParticipant GetParticipant(ulong discordId, string githubUsername)
            => new GitweekParticipant
            {
                DiscordId = discordId,
                GitHubUsername = githubUsername,
                CommitsByDate = GetCommitsFor(githubUsername)
            };

        private Dictionary<DateTime, int> GetCommitsFor(string username)
        {
            var res = new Dictionary<DateTime, int>();
            var url = $"https://github.com/{username}";
            using (var client = new WebClient ())
            {
                var source = client.DownloadString(url);
                var matches = commitRegex.Matches(source);
                foreach (Match match in matches)
                {
                    var date = DateTime.Parse(match.Groups[2].Value);
                    if (date.Date >= warmupStart.Date)
                    {
                        res.Add(date, int.Parse(match.Groups[1].Value));
                    }
                }
            }

            return res;
        }

        private bool IsGitweek()
            => DateTime.Now < gitweekStart;
    }
}