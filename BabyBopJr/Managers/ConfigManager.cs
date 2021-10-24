﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBopJr.Managers
{
    public static class ConfigManager
    {
        private static string ConfigFolder = "Java";
        private static string ConfigFile = "config.json";
        private static string ConfigPath = ConfigFolder + "/" + ConfigFile;
        public static BotConfig Config { get; private set; }

        static ConfigManager()
        {
            if (!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);

            if (!File.Exists(ConfigPath))
            {
                Config = new BotConfig();
                var json = JsonConvert.SerializeObject(Config, Formatting.Indented);
                File.WriteAllText(ConfigPath, json);
            }
            else
            {
                var json = File.ReadAllText(ConfigPath);
                Config = JsonConvert.DeserializeObject <BotConfig>(json);
            }
        }
    }



    public struct BotConfig
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
    }
}
