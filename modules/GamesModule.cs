using Unicorn.Services;
using Discord.WebSocket;
using Discord.Commands;
using Discord;

namespace Unicorn.Modules
{
    [Group("play")]
    public class GamesModule : ModuleBase<SocketCommandContext>
    {
        private readonly EmoteService emoteService;

        public GamesModule(EmoteService emoteService)
        {
            this.emoteService = emoteService;
        }

        [Command("find")]
        [Summary("Starts the Find the Snail game")]
        public async Task FindingSnailAsync()
        {
            await ReplyAsync("Find the snail!");
        }
    }
}