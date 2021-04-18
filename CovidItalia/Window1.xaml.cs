using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using CovidNotifyLib;
using LibCovid;

namespace CovidItalia
{
    /// <summary>
    /// Logica di interazione per Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            botID.IsEnabled = true;
            chatID.IsEnabled = true;
        }

        private void salva_Click(object sender, RoutedEventArgs e)
        {
            var values = Parser.Filtered(DateTime.Today);
            var value = values[0];

            if (telegram.IsChecked == true)
            {
                if (botID.Text == "")
                {
                    botID.BorderBrush = Brushes.Red;
                }
                if (chatID.Text == "")
                {
                    chatID.BorderBrush = Brushes.Red;
                }
            }
            else
            {
                List<ConfigurationFile.data> config = new List<ConfigurationFile.data>();

                config.Add(new ConfigurationFile.data()
                {
                    win = (bool)windows.IsChecked,
                    telegram = (bool)telegram.IsChecked,
                    botID = botID.Text,
                    chatID = chatID.Text,
                    lastUpdate = value.data
                });

                ConfigurationFile.WriteFile(config);

                Close();
            }
        }
    }
}
