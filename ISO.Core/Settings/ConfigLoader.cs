using ISO.Core.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\config.json";

            Log.Info("Loading config from path " + path);

            var file = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Config>(file);
        }
    }
}
