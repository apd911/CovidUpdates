using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            
            data.Text = "Aggiornamento: " + value.data;
            dati.Text = value.nuoviPositivi + "\t\tNuovi contagi\n" + value.totalePositivi + "\t\tTotale positivi\n" + value.nuoviDeceduti + "\t\tNuovi deceduti\n" +
                value.totaleDeceduti + "\t\tTotale deceduti\n" + value.nuoviTamponi + "\t\tNuovi tamponi\n" + value.totaleTamponi + "\tTotale tamponi\n" +
                value.totaleGuariti + "\tTotale guariti\n" + value.totaleCasi + "\tTotale casi\n";
        }
        private void DateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selected = (DateTime)calendar.SelectedDate;
            var values = Parser.Filtered(selected);
            var value = values[0];

            data.Text = "Aggiornamento: " + value.data;
            dati.Text = value.nuoviPositivi + "\t\tNuovi contagi\n" + value.totalePositivi + "\t\tTotale positivi\n" + value.nuoviDeceduti + "\t\tNuovi deceduti\n" +
                value.totaleDeceduti + "\t\tTotale deceduti\n" + value.nuoviTamponi + "\t\tNuovi tamponi\n" + value.totaleTamponi + "\tTotale tamponi\n" +
                value.totaleGuariti + "\tTotale guariti\n" + value.totaleCasi + "\tTotale casi\n";
        }
    }
}
