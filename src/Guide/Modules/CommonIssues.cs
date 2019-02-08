﻿using CommonIssues.JsonParser;
using CommonIssues.JsonParser.Entities;
using Discord;
using Discord.Commands;
using Guide.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Guide.Modules
{
    public class CommonIssues : ModuleBase<SocketCommandContext>
    {
        private ILogger _logger;

        public CommonIssues(ILogger logger)
        {
            _logger = logger;
        }

        [Command("check")]
        public async Task SearchIssue([Remainder]string search)
        {
            var embed = new EmbedBuilder();
            var sb = new StringBuilder();
            var client = new CommonIssuesClient();

            var searchIssue = await client.SearchCommonIssuesAsync(search);
            if (searchIssue != null)
            {
                sb.Append($"{searchIssue.Description}\n\nURL: [{searchIssue.Name}]({searchIssue.Url})");
                embed.WithColor(Color.Green)
                     .WithCurrentTimestamp()
                     .WithImageUrl(searchIssue.Img)
                     .WithTitle(searchIssue.Name)
                     .WithDescription(sb.ToString());
                await ReplyAsync("", false, embed.Build());
            }
            else
            {
                var partialSearch = await client.GetPartialMatches(search);
                if (partialSearch.Count == 1)
                {
                    sb.Append($"Nearest Result: [{partialSearch[0].Name}]({partialSearch[0].Url})" +
                        $"\n{partialSearch[0].Description}");
                    embed.WithImageUrl(partialSearch[0].Img);
                }
                else if (partialSearch.Count > 1)
                {
                    sb.Append($"Nearest Result: [{partialSearch[0].Name}]({partialSearch[0].Url})" +
                        $"\n{partialSearch[0].Description}");
                    embed.WithImageUrl(partialSearch[0].Img);
                    partialSearch.RemoveAt(0);
                    sb.Append("\n\nOther Possible Results\n");
                    partialSearch.ForEach(x => sb.Append($"URL: [{x.Name}]({x.Url})\n"));
                }
                else
                {
                    _logger.Log("Command execution failed. Reason: No result found in CommonIssue..");
                }
            }
        }
    }
}
