using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Guide.Core.Exceptions;
using Guide.Language;
using Guide.Logging;
using Ninject;

namespace Guide.Handlers
{
    public class DiscordCommandHandler : ICommandHandler
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService commandService;
        private readonly ILogger logger;
        private readonly IKernel kernel;
        private readonly ILanguage _lang;

        public DiscordCommandHandler(DiscordSocketClient client, CommandService commandService, ILogger logger, IKernel kernel, ILanguage lang)
        {
            this.client = client;
            this.commandService = commandService;
            this.logger = logger;
            this.kernel = kernel;
            _lang = lang;
        }

        public async Task InitializeAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg))
            {
                return;
            }
            
            var argPos = 0;
            if (msg.HasMentionPrefix(client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(client, msg);
                await TryRunAsBotCommand(context, argPos).ConfigureAwait(false);
            }
        }

        private async Task TryRunAsBotCommand(SocketCommandContext context, int argPos)
        {
            try
            {
                var result = await commandService.ExecuteAsync(context, argPos, kernel);
                
                if(!result.IsSuccess)
                {
                    logger.Log($"Command execution failed. Reason: {result.ErrorReason}.");
                }
            }
            catch(DomainException e)
            {
                await context.Channel.SendMessageAsync(string.Format(_lang.GetPhrase("ERROR"), e.Message));
            }
        }
    }
}
