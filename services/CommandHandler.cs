using Microsoft.Extensions.DependencyInjection;
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
        private readonly string[] prefixes;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services,
            string[] prefixes)
        {
            this.client = client;
            this.commands = commands;
            this.services = services;
            this.prefixes = prefixes;
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

            bool hasPrefix = false;
            foreach (var prefix in prefixes)
            {
                if (message.HasStringPrefix(prefix, ref argPos)) hasPrefix = true;
            }

            if (!hasPrefix || message.Author.IsBot) return;

            var context = new SocketCommandContext(client, message);

            await commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: services
            );
        }
    }
}