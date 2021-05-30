using System;
using System.Configuration;

namespace CovidItalia
{
    public class Configuration
    {
        public class Write
        {
            public static void win(bool value)
            {
                System.Configuration.Configuration confFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                confFile.AppSettings.Settings["win"].Value = value.ToString();
                confFile.Save(ConfigurationSaveMode.Modified);
            }

            public static void telegram(bool value)
            {
                System.Configuration.Configuration confFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                confFile.AppSettings.Settings["telegram"].Value = value.ToString();
                confFile.Save(ConfigurationSaveMode.Modified);
            }

            public static void botID(string value)
            {
                System.Configuration.Configuration confFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                confFile.AppSettings.Settings["botID"].Value = value;
                confFile.Save(ConfigurationSaveMode.Modified);
            }

            public static void chatID(string value)
            {
                System.Configuration.Configuration confFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                confFile.AppSettings.Settings["chatID"].Value = value;
                confFile.Save(ConfigurationSaveMode.Modified);
            }

            public static void lastUpdate(DateTime value)
            {
                System.Configuration.Configuration confFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                confFile.AppSettings.Settings["lastUpdate"].Value = value.ToString();
                confFile.Save(ConfigurationSaveMode.Modified);
            }
        }

        public class Get
        {
            public static bool win()
            {
                System.Configuration.Configuration confFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                return Convert.ToBoolean(confFile.AppSettings.Settings["win"].Value.ToString());
            }

            public static bool telegram()
            {
                System.Configuration.Configuration confFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                return Convert.ToBoolean(confFile.AppSettings.Settings["telegram"].Value.ToString());
            }

            public static string botID()
            {
                System.Configuration.Configuration confFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                return confFile.AppSettings.Settings["botID"].Value.ToString();
            }

            public static string chatID()
            {
                System.Configuration.Configuration confFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                return confFile.AppSettings.Settings["chatID"].Value.ToString();
            }

            public static DateTime lastUpdate()
            {
                System.Configuration.Configuration confFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                return Convert.ToDateTime(confFile.AppSettings.Settings["lastUpdate"].Value.ToString());
            }
        }
    }
}
