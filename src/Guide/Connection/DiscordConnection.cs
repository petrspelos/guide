using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Guide.Logging;

namespace Guide.Connection
{
    public class DiscordConnection : IConnection
    {
        private readonly DiscordSocketClient client;
        private readonly ILogger logger;

        public DiscordConnection(DiscordSocketClient client, ILogger logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public async Task Connect()
        {
            client.Log += logger.Log;
            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable(Constants.TokenVariable));
            await client.StartAsync();
        }
    }
}
