using Discord;
using Discord.Commands;
using Unicorn.Services;

namespace Unicorn.Commands
{
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        private readonly EmoteService emoteService;
        private readonly CommandHandler commandHandler;

        public BasicModule(EmoteService emoteService, CommandHandler commandHandler)
        {
            this.emoteService = emoteService;
            this.commandHandler = commandHandler;
        }

        [Command("help")]
        [Summary("Displays a list of all commands")]
        public Task HelpAsync()
        {
            var happyEmote = emoteService.emotes["UniHappy"];

            var embed = new EmbedBuilder();
            embed.WithTitle("Unicorn Dashboard")
                .WithDescription($"These are all available commands {happyEmote}")
                .WithAuthor(Context.Client.CurrentUser)
                .WithColor(Color.Purple);

            List<CommandInfo> commands = commandHandler.commandService.Commands.ToList();
            foreach (var cmdInfo in commands)
            {
                string title = $"{commandHandler.prefixes[0]}{cmdInfo.Name}";
                string description = $"{cmdInfo.Summary}"
                    ?? "No description available\n";
                embed.AddField(cmdInfo.Name, description);
            }

            return ReplyAsync(embed: embed.Build());
        }
    }
}