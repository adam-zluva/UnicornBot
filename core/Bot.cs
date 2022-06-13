using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Unicorn.Services;

namespace Unicorn.Core
{
    public class Bot
    {
        private readonly string token;

        private readonly DiscordSocketClient client;

        // Services
        private readonly CommandHandler commandHandler;
        private readonly DatabaseService databaseService;
        private readonly EmoteService emoteService;

        private const string SERVER_DATA_PATH = "./data/test_server_data.json";
        private const string SAVE_DATA_PATH = "./data/saved_data.json";

        public Bot(string token)
        {
            this.token = token;

            this.client = new DiscordSocketClient();
            this.client.Log += Log;
            this.client.Ready += Ready;

            var commandService = new CommandService();
            commandService.Log += Log;

            this.databaseService = new DatabaseService(SERVER_DATA_PATH, SAVE_DATA_PATH);
            this.emoteService = new EmoteService(databaseService);
            this.commandHandler = new CommandHandler(this.client, commandService,
                BuildServiceProvider(), databaseService.serverData.botPrefixes);
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
            if (msg.Exception is CommandException exc)
            {
                var emote = emoteService.emotes["unicornSad"];
                string text = $"Oh no! An error occured {emote}";
                exc.Context.Channel.SendMessageAsync(text);
            }

            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task Ready()
        {
            string prefixes = string.Join(",", databaseService.serverData.botPrefixes);
            await client.SetGameAsync(prefixes);

            foreach (var guild in client.Guilds)
            {
                var systemChannel = guild.GetTextChannel(databaseService.serverData.systemChannelID);
                if (systemChannel != null)
                {
                    string text = $"{databaseService.serverData.botMessages["helloWorldMessage"]}";
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