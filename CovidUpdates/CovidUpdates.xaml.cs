using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CovidLib;

namespace CovidUpdates
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            var values = Parser.Filtered(DateTime.Today);
            var value = values[0];

            regioni.Text = "-- Seleziona --";

            labels.Text = "Nuovi Contagi\n" + "Totale Positivi\n" + "Nuovi Deceduti\n" + "Totale Deceduti\n" + "Nuovi Tamponi\n" + "Totale Tamponi\n" + "Totale Guariti\n" + "Totale Casi";
            labelsr.Text = "Nuovi Contagi\n" + "Totale Positivi\n" + "Nuovi Deceduti\n" + "Totale Deceduti\n" + "Nuovi Tamponi\n" + "Totale Tamponi\n" + "Totale Guariti\n" + "Totale Casi\n" + "Regione";

            data.Text = value.data;
            datar.Text = value.data;
            dati.Text = value.nuoviPositivi + "\n" + value.totalePositivi + "\n" + value.nuoviDeceduti + "\n" +
                value.totaleDeceduti + "\n" + value.nuoviTamponi + "\n" + value.totaleTamponi + "\n" +
                value.totaleGuariti + "\n" + value.totaleCasi;
            datir.Text = value.nuoviPositivi + "\n" + value.totalePositivi + "\n" + value.nuoviDeceduti + "\n" +
                value.totaleDeceduti + "\n" + value.nuoviTamponi + "\n" + value.totaleTamponi + "\n" +
                value.totaleGuariti + "\n" + value.totaleCasi + "\nItalia";
        }
        private void DateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selected = (DateTime)calendar.SelectedDate;
            var values = Parser.Filtered(selected);
            var value = values[0];

            data.Text = value.data;
            dati.Text = value.nuoviPositivi + "\n" + value.totalePositivi + "\n" + value.nuoviDeceduti + "\n" +
                value.totaleDeceduti + "\n" + value.nuoviTamponi + "\n" + value.totaleTamponi + "\n" +
                value.totaleGuariti + "\n" + value.totaleCasi;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbi = (ComboBoxItem)regioni.SelectedItem;
            DateTime selected = (DateTime)calendarr.SelectedDate;
            string regione = "Abruzzo";
            if (cbi != null) { regione = cbi.Content.ToString(); }

            var valuesr = Parser.FilteredRegioni(selected, regione);
            var valuer = valuesr[0];

            datar.Text = valuer.data;
            datir.Text = valuer.nuoviPositivi + "\n" + valuer.totalePositivi + "\n" + valuer.nuoviDeceduti + "\n" +
               valuer.totaleDeceduti + "\n" + valuer.nuoviTamponi + "\n" + valuer.totaleTamponi + "\n" +
               valuer.totaleGuariti + "\n" + valuer.totaleCasi + "\n" + valuer.regione;
        }
    }
}
