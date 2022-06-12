using Discord;

namespace Unicorn.Services
{
    public class EmoteService
    {
        private readonly DatabaseService databaseService;
        public readonly Dictionary<string, Emote> emotes;

        public EmoteService(DatabaseService databaseService)
        {
            this.databaseService = databaseService;

            this.emotes = new Dictionary<string, Emote>();
            var emotesData = this.databaseService.serverData.emotesData;
            foreach (var emoteData in emotesData)
            {
                string key = emoteData.Key;
                if (Emote.TryParse(emoteData.Value, out Emote emote))
                {
                    emotes.Add(key, emote);
                }
            }
        }
    }
}