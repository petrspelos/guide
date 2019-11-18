using Discord.WebSocket;
using Guide.Connection;
using Guide.Handlers;
using Guide.Json;
using Guide.Logging;
using Guide.Language;
using Guide.Services;
using Ninject;
using Ninject.Modules;
using Discord;

namespace Guide
{
    internal sealed class Startup
    {
        internal readonly IKernel Kernel;

        internal Startup()
        {
            Kernel = new StandardKernel(new GuideModule());
        } 
    }

    internal sealed class GuideModule : NinjectModule
    {
        public override void Load()
        {
            var client = new DiscordSocketClient(new DiscordSocketConfig{ LogLevel = LogSeverity.Verbose });

            Bind<Connection.IConnection>().To<DiscordConnection>();
            Bind<ICommandHandler>().To<DiscordCommandHandler>();
            Bind<ILogger>().To<ConsoleLogger>();
            Bind<IJsonStorage>().To<JsonStorage>().InSingletonScope();
            Bind<ILanguage>().To<JsonLanguage>().InSingletonScope();
            Bind<WelcomeMessageService>().ToSelf().InSingletonScope();
            Bind<DiscordSocketClient>().ToMethod(context => client).InSingletonScope();
        }
    }
}
