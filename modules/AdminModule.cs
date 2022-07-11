using Discord.Commands;

namespace Unicorn.Modules
{
    [RequireOwner]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("crash")]
        [Summary("%hide% Tests a crash logging")]
        public Task CrashAsync()
        {
            throw new NotImplementedException();
        }
    }
}