using YamlDotNet.Serialization;

namespace TribalWarsBot
{
    /** CURRENT PLAYABLE WORLD DATA */
    /** Loads WorldConfig.yaml file */
    public class TWConfig
    {
        private static TWConfig? _config = null;
        public static TWConfig GetTWConfig()
        {
            if (_config == null)
            {
                IDeserializer deserializer = new DeserializerBuilder().Build();
                _config = deserializer.Deserialize<TWConfig>(File.ReadAllText("config/WorldConfig.yaml"));
            }
            return _config;
        }
        [YamlMember(Alias = "speed", ApplyNamingConventions = false)]
        public double WORLD_SPEED { get; set; } = 1;
        [YamlMember(Alias = "player-name", ApplyNamingConventions = false)]
        public String WORLD_PLAYER_NAME { get; set; } = "";
        [YamlMember(Alias = "player-pass", ApplyNamingConventions = false)]
        public String WORLD_PLAYER_PASS { get; set; } = "";
        [YamlMember(Alias = "mainpage-url", ApplyNamingConventions = false)]
        public String GAME_URL { get; set; } = "";
        [YamlMember(Alias = "world-name", ApplyNamingConventions = false)]
        public String WORLD_NAME { get; set; } = "";
        [YamlMember(Alias = "world-url", ApplyNamingConventions = false)]
        public String WORLD_URL { get; set; } = "";
        [YamlMember(Alias = "captcha-token", ApplyNamingConventions = false)]
        public String CAPTCHA_TOKEN { get; set; } = "";
    }
}
