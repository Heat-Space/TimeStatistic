using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeStatistic
{
    public class Config
    {
        
        public static string SavePath
        {
            get
            {
                string path = @"tshock\TimeStatistic";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return Path.Combine(path, "settings.json");
            }
        }

        public DateTime wipeStart = DateTime.MinValue;
        public bool updateTime = true;

        public void Write()
        {
            File.WriteAllText(SavePath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }
        public static Config Read()
        {
            Config config = new();
            if (!File.Exists(SavePath))
            {
                config.Write();
                return config;
            }
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(SavePath));
            config.Write();
            return config;
        }
    }
}
