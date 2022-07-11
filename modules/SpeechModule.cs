using Unicorn.Services;
using Discord.WebSocket;
using Discord.Commands;

namespace Unicorn.Modules
{
    public class SpeechModule : ModuleBase<SocketCommandContext>
    {
        private readonly EmoteService emoteService;

        public SpeechModule(EmoteService emoteService)
        {
            this.emoteService = emoteService;
        }

        [Command("say")]
        [Alias("s")]
        [Summary("%hide%Repeats a message")]
        public Task SayAsync([Remainder][Summary("The text to repeat")] string text)
        {
            Context.Message.DeleteAsync();
            return ReplyAsync(text);
        }

        [Command("say")]
        [Alias("s")]
        [Summary("%hide%Repeats a message in specific channel")]
        public Task SayAsync([Summary("The channel to repeat the message to")] SocketTextChannel channel,
            [Remainder][Summary("The text to repeat")] string text)
        {
            return channel.SendMessageAsync(text);
        }

        [Command("dallin")]
        [Summary("%hide%Ballin")]
        public Task DallinAsync()
        {
            var dallinEmoji = emoteService.emotes["Dallin"];
            return ReplyAsync($"{dallinEmoji}⚾ Dallin is ballin.");
        }
    }
}