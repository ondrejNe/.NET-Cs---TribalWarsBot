using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TribalWarsBot
{
    public class ScavengeVillages
    {
        private static readonly string SCAVENGE_CONFIG_PATH = "config/ScavengeConfig.yaml";
        /** List of specified villages*/
        public List<ScavengeVillage>? Villages { get; set; }
        /** Loads the scavenge config in ScavengeConfig.yaml file */
        public static ScavengeVillages LoadScavengeConfig()
        {
            IDeserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            ScavengeVillages ret;
            lock (Dispatcher.GetConfigLock()){
                ret = deserializer.Deserialize<ScavengeVillages>(File.ReadAllText(SCAVENGE_CONFIG_PATH));
            }
            return ret;
        }
    }
    public class ScavengeVillage
    {
        public string Name { get; set; } = "";
        public int Id { get; set; }
        public int Coordx { get; set; }
        public int Coordy { get; set; }
        public string Spear { get; set; } = "";
        public string Sword { get; set; } = "";
        public string Axe { get; set; } = "";
        public string Archer { get; set; } = "";
        public string Light { get; set; } = "";
        public string Marcher { get; set; } = "";
        public string Heavy { get; set; } = "";
        /** Returns a list of allowed counts per units */
        // Takes list of available units
        public List<int> GetAllowedCounts(List<int> totalCounts)
        {
            List<int> counts = new()
            {
                ParseCount(totalCounts[0], Spear),
                ParseCount(totalCounts[1], Sword),
                ParseCount(totalCounts[2], Axe),
                ParseCount(totalCounts[3], Archer),
                ParseCount(totalCounts[4], Light),
                ParseCount(totalCounts[5], Marcher),
                ParseCount(totalCounts[6], Heavy)
            };
            return counts;
        }
        /** Returns a number of allowed units based on total count and parsed modifier */
        public static int ParseCount(int totalCount, string allowedCount)
        {
            // unit is not allowed
            if (allowedCount == "none") return 0;
            // unit is allowed 
            if (allowedCount == "all") return totalCount;
            // unit is allowed but <number> of it must be omitted
            if (allowedCount.StartsWith("all"))
            {
                string s = allowedCount.Split(" ")[1];
                int omit = int.Parse(s);
                return Math.Max(totalCount - omit, 0);
            }
            // unit is allowed in percentage e.g. 90% of total units
            if (allowedCount.EndsWith("%"))
            {
                string s = allowedCount.Replace("%", "");
                double mult = double.Parse(s) / 100;
                return Convert.ToInt32(Math.Floor(totalCount * mult));
            }
            // unit is allowed only to specified max <number>
            int allowed = int.Parse(allowedCount);
            return totalCount >= allowed ? allowed : totalCount;
        }
    }
}
