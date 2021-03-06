using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Interactions;
using Unicorn.Services;

namespace Unicorn.Core
{
    public class Bot
    {
        private readonly string token;

        private readonly DiscordSocketClient client;

        // Services
        private readonly CommandService commandService;
        private readonly CommandHandler commandHandler;
        private readonly DatabaseService databaseService;
        private readonly EmoteService emoteService;

        private const string SERVER_DATA_PATH = "./data/config.json";
        private const string SAVE_DATA_PATH = "./data/saved_data.json";

        public Bot(string token)
        {
            this.token = token;

            this.client = new DiscordSocketClient();
            this.client.Log += Log;
            this.client.Ready += Ready;

            this.commandService = new CommandService();

            this.databaseService = new DatabaseService(SERVER_DATA_PATH, SAVE_DATA_PATH);
            this.emoteService = new EmoteService(databaseService);
            this.commandHandler = new CommandHandler(client, commandService,
                BuildServiceProvider(), databaseService.config.botPrefixes);
        }

        public async Task BootAsync()
        {
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await commandHandler.InstallCommandsAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task Ready()
        {
            string prefixes = string.Join(",", databaseService.config.botPrefixes);
            await client.SetGameAsync(prefixes);

            foreach (var guild in client.Guilds)
            {
                var systemChannel = guild.GetTextChannel(databaseService.config.systemChannelID);
                if (systemChannel != null)
                {
                    string text = $"{databaseService.config.botMessages["helloWorldMessage"]}";
                    await systemChannel.SendMessageAsync(text);
                    break;
                }
            }
        }

        public IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(databaseService);
            serviceCollection.AddSingleton(emoteService);
            return serviceCollection.BuildServiceProvider();
        }
    }
}