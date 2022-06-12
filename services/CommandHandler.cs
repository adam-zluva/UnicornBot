using System.Reflection;
using Discord.WebSocket;
using Discord.Commands;

namespace Unicorn.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly IServiceProvider services;
        private readonly char prefix;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            this.client = client;
            this.commands = commands;
            this.services = services;
            this.prefix = '>';
        }

        public async Task InstallCommandsAsync()
        {
            await commands.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: services);

            client.MessageReceived += HandleCommandAsync;
        }

        public async Task HandleCommandAsync(SocketMessage messageContext)
        {
            var message = messageContext as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            if (!(message.HasCharPrefix(prefix, ref argPos) ||
                message.HasMentionPrefix(client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(client, message);

            await commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: services
            );
        }
    }
}