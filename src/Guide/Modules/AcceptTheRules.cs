using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Guide.Core.Boundaries.AcceptTheRules;
using Guide.Language;
using Guide.Preconditions;

namespace Guide.Modules
{
    [RequireChannelId(Constants.WaitingRoomId)]
    public class AcceptTheRules : ModuleBase<SocketCommandContext>, IAcceptTheRulesOutputPort
    {
        private readonly IAcceptTheRules _useCase;
        private readonly ILanguage _lang;

        public AcceptTheRules(IAcceptTheRules useCase, ILanguage lang)
        {
            _useCase = useCase;
            _useCase.Output = this;
            _lang = lang;
        }

        [Command("I accept the rules", RunMode = RunMode.Async)]
        [Alias("I accept the rules.")]
        public async Task AcceptRules([Remainder]string bio = "The user didn't provide this information.")
        {
            if (!(Context.User is SocketGuildUser user) || UserIsMember(user)) { return; }

            var visibleName = user.Nickname ?? user.Username;

            await _useCase.Execute(new AcceptTheRulesInput(Context.User.Id, visibleName, bio));
        }
        public async void Default(AcceptTheRulesOutput output)
        {
            var embed = new EmbedBuilder()
                .WithTitle(string.Format(_lang.GetPhrase(Constants.PKUserJoinedTitle), output.AcceptedName))
                .WithColor(Constants.PrimaryColor)
                .AddField("About this user", output.Bio)
                .Build();

            var general = Context.Guild.GetTextChannel(Constants.GeneralId);

            await general.SendMessageAsync("", embed: embed);
        }

        public async void Error(string message)
        {
            if(message == "Invalid Username")
                await ReplyAsync(_lang.GetPhrase("INVALID_NAME"));
        }

        private static bool UserIsMember(SocketGuildUser user)
            => user.Roles.Any(r => r.Id == Constants.MemberRoleId);
    }
}

