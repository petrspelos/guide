using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Guide.GitWeek;

namespace Guide.Modules
{
    public class GitWeek : ModuleBase<SocketCommandContext>
    {
        private readonly IGitUserVerification _gitUserVerification;
        private readonly IGitweekStats _gitweekStats;
        private readonly DiscordSocketClient _client;

        public GitWeek(IGitUserVerification gitUserVerification, IGitweekStats gitweekStats, DiscordSocketClient client)
        {
            _gitUserVerification = gitUserVerification;
            _gitweekStats = gitweekStats;
            _client = client;
        }
        
        [Command("github register")]
        public async Task RegisterGitHub(string username)
        {
            try
            {
                var hash = await _gitUserVerification.GetVerificationStringFor(Context.User.Id, username);
                await ReplyAsync($"In order to verify that the account is yours, go to your profile and add the following hash to your Bio:\n```\n{hash}\n```\nthen use the `github verify {username}` command.");
            }
            catch (Exception e)
            {
                await ReplyAsync($":x: {e.Message}");
            }
        }

        [Command("github verify")]
        public async Task VerifyGitHub(string username)
        {
            try
            {
                var success = await _gitUserVerification.Verify(Context.User.Id, username);
                if (success)
                {
                    await ReplyAsync("Congratulations! :tada: Your GitHub name was verified!\nYou can now remove the hash from your Bio.");
                }
            }
            catch (Exception e)
            {
                await ReplyAsync($":x: {e.Message}");
            }
        }

        [Command("refresh score")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RefreshScore()
        {
            try
            {
                await ReplyAsync("I'm on it. It will take some time in order to not spam GitHub (and get destroyed by Microsoft as a result).\n\nOh and Discord will not be happy with me posting so much stuff at once, so I'll go checkout r/unixporn each time they rate limit me. We could switch our IP and pretend to be a different shard but that sounds like work.");
                var tutorialGuild = _client.GetGuild(Constants.TutorialGuildId);
                var channel = tutorialGuild.GetTextChannel(Constants.ScoreboardId);
                var users = _gitweekStats.GetLeaderboards();
                foreach(var user in users)
                    await channel.SendMessageAsync(user);
            }
            catch (Exception e)
            {
                await ReplyAsync($":x: {e.Message}");
            }
        }
    }
}
