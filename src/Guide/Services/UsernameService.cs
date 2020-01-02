using System.IO;
using System.Threading.Tasks;
using Discord.WebSocket;
using Guide.Extensions;

namespace Guide.Services
{
    public class UsernameService
    {
        private readonly DiscordSocketClient _client;

        public UsernameService(DiscordSocketClient client)
        {
            _client = client;

            client.UserUpdated += UserUpdated;
        }

        private async Task UserUpdated(SocketUser before, SocketUser after)
        {
            var guildUser = _client.GetGuild(Constants.TutorialGuildId).GetUser(after.Id);

            if (!string.IsNullOrEmpty(guildUser.Nickname)) return;

            if (!string.IsNullOrEmpty(after.Username) && after.Username[0].IsAsciiPrintable()) return;

            var randomName = File.ReadAllLines(Constants.NamesFile).GetRandomElement();

            await guildUser.ModifyAsync(u => u.Nickname = randomName);
        }
    }
}
