using Newtonsoft.Json;

namespace Unicorn.Services
{
    public class DatabaseService
    {
        public readonly ConfigData config;

        public DatabaseService(string configPath, string saveDataPath)
        {
            string jsonConfig = File.ReadAllText(configPath);
            this.config = JsonConvert.DeserializeObject<ConfigData>(jsonConfig)!;
        }
    }

    // The JSON file must have the same structure as this class
    // JSON objects get deserialized as Dictionaries
    public class ConfigData
    {
        public Dictionary<string, string> botMessages;
        public string[] botPrefixes;
        public string[] emotes;
        public ulong systemChannelID;
        public ulong managerUserID;
    }
}