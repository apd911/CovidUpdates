using System.Windows;
using System.Windows.Media;

namespace CovidItalia
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            botID.Text = Configuration.Get.botID();
            chatID.Text = Configuration.Get.chatID();
            windows.IsChecked = Configuration.Get.win();
            telegram.IsChecked = Configuration.Get.telegram();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            botID.IsEnabled = true;
            chatID.IsEnabled = true;
        }

        private void salva_Click(object sender, RoutedEventArgs e)
        {
            if (telegram.IsChecked == true & (botID.Text == "" | chatID.Text == ""))
            {
                botID.BorderBrush = Brushes.Red;
                chatID.BorderBrush = Brushes.Red;
            }
            else
            {
                Configuration.Write.win(windows.IsChecked.Value);
                Configuration.Write.telegram(telegram.IsChecked.Value);
                Configuration.Write.botID(botID.Text.ToString());
                Configuration.Write.chatID(chatID.Text.ToString());
                Close();
            }
        }
    }
}