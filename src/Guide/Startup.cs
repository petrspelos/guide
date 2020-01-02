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
using Guide.Core.Boundaries.ToggleHelperRole;
using Guide.Core.UseCases;
using Guide.Core.Services;
using Guide.Core.Boundaries.AcceptTheRules;
using System;

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
            Bind<DiscordSocketClient>().ToMethod(context => client).InSingletonScope();
            Bind<IToggleHelperRole>().To<ToggleHelperRole>();
            Bind<IAcceptTheRules>().To<AcceptTheRules>();
            Bind<IRoleService>().To<RoleService>();
            Bind<INameValidator>().To<NameValidator>();
            Bind<WelcomeMessageService>().ToSelf().InSingletonScope();
            Bind<UsernameService>().ToSelf().InSingletonScope();
            Bind<Guide>().ToSelf().OnActivation(guide =>
            {
                Kernel.Get<WelcomeMessageService>();
                Kernel.Get<UsernameService>();
            });
        }
    }
}
