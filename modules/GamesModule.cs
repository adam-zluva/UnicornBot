using Unicorn.Services;
using Discord.WebSocket;
using Discord.Commands;
using Discord;

namespace Unicorn.Commands
{
    [Group("play")]
    public class GamesModule : ModuleBase<SocketCommandContext>
    {
        private readonly EmoteService emoteService;

        public GamesModule(EmoteService emoteService)
        {
            this.emoteService = emoteService;
        }

        [Command("find-the-snail")]
        [Summary("Starts the Find the Snail game")]
        public async Task FindingSnailAsync()
        {
            var componentBuilder = new ComponentBuilder()
                .WithButton("Here!", "game");

            await ReplyAsync("Find the snail!", components: componentBuilder.Build());
        }
    }
}