using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Linq;
using Guide.Language;
using Guide.Core.Boundaries.ToggleHelperRole;

namespace Guide.Modules
{
    public class Helpdesk : ModuleBase<SocketCommandContext>, IToggleHelperRoleOutputPort
    {
        private readonly IToggleHelperRole _useCase;
        private readonly ILanguage lang;

        public Helpdesk(IToggleHelperRole useCase, ILanguage lang)
        {
            _useCase = useCase;
            _useCase.Output = this;
            this.lang = lang;
        }

        [Command("helper")]
        public async Task ToggleHelper()
        {
            await _useCase.Execute(new ToggleHelperRoleInput(Context.User.Id));
        }

        public async void Default(ToggleHelperRoleOutput output)
        {
            var phrase = output.RoleAdded ? "HELPER_ADDED" : "HELPER_REMOVED";

            await ReplyAsync(lang.GetPhrase(phrase));
        }

        public async void Error(string message)
        {
            await ReplyAsync(string.Format(lang.GetPhrase("ERROR"), message));
        }
    }
}