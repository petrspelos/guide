using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Guide.Core.Services;

namespace Guide.Services
{
    public sealed class RoleService : IRoleService
    {
        private readonly DiscordSocketClient _client;

        public RoleService(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task AssignHelperRole(ulong id)
            => await AssignRole(id, Constants.HelperRoleId);

        public async Task AssignMemberRole(ulong id)
            => await AssignRole(id, Constants.MemberRoleId);

        public async Task UnassignHelperRole(ulong id)
        {
            var guild = _client.GetGuild(Constants.TutorialGuildId);
            var user = guild.GetUser(id);
            var role = guild.GetRole(Constants.HelperRoleId);

            await user.RemoveRoleAsync(role);
        }

        public Task<bool> UserIsHelper(ulong id)
        {
            var guild = _client.GetGuild(Constants.TutorialGuildId);
            var user = guild.GetUser(id);
            var role = guild.GetRole(Constants.HelperRoleId);

            return Task.FromResult(user.Roles.Contains(role));
        }

        private async Task AssignRole(ulong userId, ulong roleId)
        {
            var guild = _client.GetGuild(Constants.TutorialGuildId);
            var user = guild.GetUser(userId);
            var role = guild.GetRole(roleId);

            await user.AddRoleAsync(role);
        }
    }
}
