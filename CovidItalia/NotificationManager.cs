using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CovidItalia
{
    public class NotificationService
    {
        public static async void Notification()
        {
            var values = Parser.Filtered(DateTime.Today);
            var value = values[0];
            bool win = Configuration.Get.win();
            bool telegram = Configuration.Get.telegram();
            string botID = Configuration.Get.botID();
            string chatID = Configuration.Get.chatID();
            DateTime lastUpdate = Configuration.Get.lastUpdate();
            string format = "#,##0";

            Debug.WriteLine("Il worker funziona. Ore {date}", DateTime.Now.ToString("t"));

            if (lastUpdate.ToString("dd MMM yyyy") != value.data.ToString("dd MMM yyyy"))
            {
                if (win == true)
                {
                    new ToastContentBuilder()
                        .AddText("Aggiornamento CoViD-19 " + value.data.ToString("dd MMMM yyyy"))
                        .AddText("Nuovi Positivi: " + value.nuoviPositivi.ToString(format) + " Nuovi Guariti " + value.nuoviGuariti.ToString(format))
                        .AddText("Nuovi Deceduti: " + value.nuoviDeceduti.ToString(format) + " Nuovi Tamponi " + value.nuoviTamponi.ToString(format))
                        .Show();
                }

                if (telegram == true)
                {
                    var botClient = new TelegramBotClient(botID);
                    string message = "*Aggiornamento CoViD-19 " + value.data.ToString("dd MMMM yyyy") + "*" +
                        "\n\n_Nuovi Positivi:_ " + value.nuoviPositivi.ToString(format) +
                        "\n_Nuovi Guariti:_ " + value.nuoviGuariti.ToString(format) +
                        "\n_Nuovi Deceduti:_ " + value.nuoviDeceduti.ToString(format) +
                        "\n_Nuovi Tamponi:_ " + value.nuoviTamponi.ToString(format) +
                        "\n\n_Totale Positivi:_ " + value.totalePositivi.ToString(format) +
                        "\n_Totale Guariti:_ " + value.totaleGuariti.ToString(format) +
                        "\n_Totale Deceduti:_ " + value.totaleDeceduti.ToString(format) +
                        "\n_Totale Tamponi:_ " + value.totaleTamponi.ToString(format) +
                        "\n\n_Totale Casi:_ " + value.totaleCasi.ToString(format);
                    await botClient.SendTextMessageAsync(
                        chatId: chatID,
                        parseMode: ParseMode.Markdown,
                        text: message
                        );
                }

                if (win == true | telegram == true)
                {
                    Configuration.Write.lastUpdate(value.data);
                }
            }
        }
    }
}