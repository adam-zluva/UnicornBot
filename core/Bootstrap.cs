namespace Unicorn.Core
{
    public class Bootstrap
    {
        private const string TOKEN_PATH = "./secret/token.txt";

        public static Task Main(string[] args)
        {
            string token = File.ReadAllText(TOKEN_PATH);

            Bot bot = new Bot(token);
            return bot.BootAsync();
        }
    }
}