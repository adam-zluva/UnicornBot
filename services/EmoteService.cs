using System.Text.RegularExpressions;
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
            Regex rx = new Regex(@":[a-zA-Z]+:");
            var emotesUnicode = this.databaseService.serverData.emotes;
            foreach (var emote in emotesUnicode)
            {
                string key = rx.Match(emote).Value.Replace(":", "");
                if (Emote.TryParse(emote, out Emote result))
                {
                    emotes.Add(key, emote);
                }
            }
        }
    }
}