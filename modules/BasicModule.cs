using Unicorn.Services;
using Discord.Commands;

namespace Unicorn.Commands
{
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        private readonly EmoteService emojiService;

        public BasicModule(EmoteService emojiService)
        {
            this.emojiService = emojiService;
        }

        [Command("say")]
        [Summary("Repeats a message")]
        public Task SayAsync([Remainder][Summary("The text to repeat")] string text)
        {
            var emoji = emojiService.emotes["unicornHappy"];
            string replyText = $"{emoji} {text}";
            return ReplyAsync(replyText, messageReference: Context.Message.GetReference());
        }

        [Command("joke")]
        [Summary("Tells a joke")]
        public Task JokeAsync()
        {
            var emoji = emojiService.emotes["unicornGrin"];
            string joke = "Youre mom";
            string replyText = $"{emoji} {joke}";
            return ReplyAsync(replyText, messageReference: Context.Message.GetReference());
        }
    }
}