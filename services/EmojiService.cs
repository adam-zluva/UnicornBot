namespace Unicorn.Services
{
    public class EmojiService
    {
        private readonly DatabaseService databaseService;

        public EmojiService(DatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }
    }
}