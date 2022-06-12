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

        private const string SERVER_DATA_PATH = "./data/test_server_data.json";
        private const string SAVE_DATA_PATH = "./data/saved_data.json";

        public Bot(string token)
        {
            this.token = token;

            this.client = new DiscordSocketClient();
            this.client.Log += Log;

            this.databaseService = new DatabaseService(SERVER_DATA_PATH, SAVE_DATA_PATH);
            this.commandHandler = new CommandHandler(this.client, new CommandService(), BuildServiceProvider());
        }

        public async Task MainAsync()
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

        public IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(databaseService);
            return serviceCollection.BuildServiceProvider();
        }
    }
}