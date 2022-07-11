using System.Text.RegularExpressions;
using Discord;
using Discord.Commands;
using Unicorn.Services;

namespace Unicorn.Modules
{
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        private readonly EmoteService emoteService;
        private readonly CommandService commandService;
        private readonly DatabaseService databaseService;

        public BasicModule(EmoteService emoteService, CommandService commandService, DatabaseService databaseService)
        {
            this.emoteService = emoteService;
            this.commandService = commandService;
            this.databaseService = databaseService;
        }

        [Command("help")]
        [Summary("Displays a list of all commands")]
        public Task HelpAsync()
        {
            var happyEmote = emoteService.emotes["UniHappy"];
            var sadEmote = emoteService.emotes["UniSad"];

            string prefix = databaseService.config.botPrefixes[0];

            Regex rx = new Regex(@"^%hide%");

            var embed = new EmbedBuilder();
            embed.WithTitle("Unicorn Commands")
                .WithDescription($"These are all available commands {happyEmote}")
                .WithAuthor(Context.Client.CurrentUser)
                .WithColor(Color.Purple);

            List<CommandInfo> commands = commandService.Commands.ToList();
            foreach (var cmdInfo in commands)
            {
                if (rx.IsMatch(cmdInfo.Summary)) continue;

                string title = $"{prefix}{cmdInfo.Name}";
                string description = $"{cmdInfo.Summary}"
                    ?? $"No description available {sadEmote}\n";
                embed.AddField(title, description);
            }

            return ReplyAsync(embed: embed.Build());
        }
    }
}