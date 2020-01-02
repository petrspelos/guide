using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Guide.Language;

namespace Guide.Services
{
    public class WelcomeMessageService
    {
        private readonly ILanguage lang;

        public WelcomeMessageService(DiscordSocketClient client, ILanguage lang)
        {
            this.lang = lang;

            client.UserJoined += UserJoined;
        }

        private async Task UserJoined(SocketGuildUser user)
        {
            await Task.Delay(2000);

            var embed = new EmbedBuilder()
                .WithTitle(lang.GetPhrase(Constants.PKWelcomeTitle))
                .AddField("WHILE YOU WAIT", lang.GetPhrase(Constants.PKWhileYouWait))
                .AddField("FUN SERVER FACT", lang.GetPhrase(Constants.PKFunServerFact))
                .Build();

            var waitingRoom = user.Guild.GetTextChannel(Constants.WaitingRoomId);

            await waitingRoom.SendMessageAsync(user.Mention, embed: embed);
        }
    }
}

