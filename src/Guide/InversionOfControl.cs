using Discord.WebSocket;
using Guide.Configuration;
using Guide.Connection;
using Guide.GitWeek;
using Guide.Handlers;
using Guide.Json;
using Guide.Logging;
using Guide.Language;
using Lamar;
using Guide.Services;
using Octokit;
using IConnection = Guide.Connection.IConnection;

namespace Guide
{
    public static class InversionOfControl
    {
        private static Container container;

        public static Container Container
        {
            get
            {
                return GetOrInitContainer();
            }
        }

        private static Container GetOrInitContainer()
        {
            if(container is null)
            {
                InitializeContainer();
            }

            return container;
        }

        public static void InitializeContainer()
        {
            container = new Container(c =>
            {
                c.For<IConnection>().Use<DiscordConnection>();
                c.For<IConfiguration>().Use<ConfigManager>();
                c.For<ICommandHandler>().Use<DiscordCommandHandler>();
                c.For<ILogger>().Use<ConsoleLogger>();
                c.For<IGitUserVerification>().Use<GitUserVerification>();
                c.For<IGitweekStats>().Use<GitweekStats>();
                c.ForSingletonOf<IJsonStorage>().UseIfNone<JsonStorage>();
                c.ForSingletonOf<ILanguage>().UseIfNone<JsonLanguage>();
                c.ForSingletonOf<WelcomeMessageService>().UseIfNone<WelcomeMessageService>();
                c.ForSingletonOf<DiscordSocketClient>().UseIfNone(DiscordSocketClientFactory.GetDefault());
                c.ForSingletonOf<GitHubClient>().UseIfNone(GithubClientFactory.GetDefault());
            });
        }
    }
}
