using Newtonsoft.Json;

namespace Unicorn.Services
{
    public class DatabaseService
    {
        public readonly ServerData serverData;

        public DatabaseService(string dataPath, string saveDataPath)
        {
            string jsonServerData = File.ReadAllText(dataPath);
            this.serverData = JsonConvert.DeserializeObject<ServerData>(jsonServerData)!;
        }
    }

    // The JSON file must have the same structure as this class
    // JSON objects get deserialized as Dictionaries
    public class ServerData
    {
        public Dictionary<string, string> botMessages;
        public string[] botPrefixes;
        public Dictionary<string, string> emotesData;
        public ulong systemChannelID;
    }
}