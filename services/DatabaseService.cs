using Newtonsoft.Json;

namespace Unicorn.Services
{
    public class DatabaseService
    {
        public readonly ServerData serverData;

        public DatabaseService(string dataPath, string saveDataPath)
        {
            string jsonServerData = File.ReadAllText(dataPath);
            this.serverData = JsonConvert.DeserializeObject<ServerData>(jsonServerData)!; // NOTE: Not ideal ツ
        }
    }

    public class ServerData
    {
        public Dictionary<string, string> emojiData;
    }
}