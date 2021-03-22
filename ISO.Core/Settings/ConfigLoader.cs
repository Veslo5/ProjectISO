using ISO.Core.Engine.Logging;
using Newtonsoft.Json;
using System;
using System.IO;

namespace ISO.Core.Settings
{
    public class ConfigLoader
    {
        /// <summary>
        /// Load config file
        /// </summary>
        /// <returns></returns>
        public static Config LoadConfig()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

            Log.Info("Loading config from path " + path, LogModule.CR);

            var file = File.ReadAllText(path);
            var config = JsonConvert.DeserializeObject<Config>(file);
            config.DataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "galaxy_data.DAT");

            return config;
        }
    }
}
