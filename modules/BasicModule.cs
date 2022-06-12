using Unicorn.Services;
using Discord.Commands;

namespace Unicorn.Commands
{
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        private readonly DatabaseService databaseService;

        public BasicModule(DatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        [Command("hello")]
        [Summary("Repeats a message")]
        public Task SayAsync([Remainder][Summary("The text to repeat")] string text)
        {
            string emoji = "";
            string replyText = $"{emoji} {text}";
            return ReplyAsync(replyText, messageReference: Context.Message.GetReference());
        }
    }
}