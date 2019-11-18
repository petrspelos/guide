using Discord.Commands;
using System.Threading.Tasks;
using Guide.Language;
using Guide.Core.Boundaries.ToggleHelperRole;

namespace Guide.Modules
{
    public class ToggleHelperRole : ModuleBase<SocketCommandContext>, IToggleHelperRoleOutputPort
    {
        private readonly IToggleHelperRole _useCase;
        private readonly ILanguage _lang;

        public ToggleHelperRole(IToggleHelperRole useCase, ILanguage lang)
        {
            _useCase = useCase;
            _useCase.Output = this;
            _lang = lang;
        }

        [Command("helper")]
        public async Task ToggleHelper()
        {
            await _useCase.Execute(new ToggleHelperRoleInput(Context.User.Id));
        }

        public async void Default(ToggleHelperRoleOutput output)
        {
            var phrase = output.RoleAdded ? "HELPER_ADDED" : "HELPER_REMOVED";

            await ReplyAsync(_lang.GetPhrase(phrase));
        }

        public async void Error(string message)
        {
            await ReplyAsync(string.Format(_lang.GetPhrase("ERROR"), message));
        }
    }
}