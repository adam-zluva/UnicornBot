using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Discord.WebSocket;
using Discord.Commands;

namespace Unicorn.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient client;
        public readonly CommandService commandService;
        private readonly IServiceProvider services;
        public readonly string[] prefixes;

        public CommandHandler(DiscordSocketClient client, CommandService commandService, IServiceProvider services,
            string[] prefixes)
        {
            this.client = client;
            this.commandService = commandService;
            this.services = services;
            this.prefixes = prefixes;
        }

        public async Task InstallCommandsAsync()
        {
            await commandService.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: services);

            client.MessageReceived += HandleCommandAsync;
            client.ButtonExecuted += HandleButtonRespond;
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

            var result = await commandService.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: services
            );

            if (result.Error != null)
            {
                var emoteService = services.GetService<EmoteService>()!;
                var databaseService = services.GetService<DatabaseService>()!;

                var emote = emoteService.emotes["UniSad"];
                string text = $"Oh no! Something went wrong {emote} " +
                    $"{result.ErrorReason}\n";
                await message.Channel.SendMessageAsync(text);
            }
        }

        public async Task HandleButtonRespond(SocketMessageComponent component)
        {
            switch (component.Data.CustomId)
            {
                case "game":
                    await component.RespondAsync($"{component.User.Mention} has clicked the buttom.");
                    break;
                default:
                    break;
            }
        }
    }
}