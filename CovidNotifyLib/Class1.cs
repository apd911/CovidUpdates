using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LibCovid;
using Json.Net;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CovidNotifyLib
{
    public class Notification
    {
        public static void Windows()
        {

        }
    }

    public class ConfigurationFile
    {
        public class data
        {
            public bool win { get; set; }
            public bool telegram { get; set; }
            public string chatID { get; set; }
            public string botID { get; set; }
            public DateTime lastUpdate { get; set; }
        }

        public static void NewFile()
        {
            List<data> _data = new List<data>();
            _data.Add(new data()
            {
                win = false,
                telegram = false,
                chatID = "",
                botID = "",
                lastUpdate = DateTime.Parse("26/04/2012")
            });

            FileStream file = File.Create(@"./config.json");
            JsonSerializer.SerializeAsync(file, _data);
            file.Close();
        }

        public static List<data> GetFile()
        {
            List<data> config = new List<data>();

            using (StreamReader r = new StreamReader("./config.json"))
            {
                string json = r.ReadToEnd();
                config = JsonNet.Deserialize<List<data>>(json);
            }

            return config;
        }

        public static void WriteFile(List<data> config)
        {
            List<data> _data = new List<data>();
            _data.Add(new data()
            {
                win = config[0].win,
                telegram = config[0].telegram,
                chatID = config[0].chatID,
                botID = config[0].botID,
                lastUpdate = config[0].lastUpdate
            });

            FileStream file = File.OpenWrite(@"./config.json");
            JsonSerializer.SerializeAsync(file, _data);
            file.Close();
        }
    }
}
