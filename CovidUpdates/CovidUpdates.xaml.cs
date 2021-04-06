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
            string source = "https://raw.githubusercontent.com/pcm-dpc/COVID-19/master/dati-json/dpc-covid19-ita-andamento-nazionale.json";
            Values value = Parser.Format(source);

            InitializeComponent();
            
            data.Text = "Aggiornato al " + value.data;
            dati.Text = value.nuoviPositivi + "\t\tNuovi contagi\n" + value.totalePositivi + "\t\tTotale positivi\n" + value.nuoviDeceduti + "\t\tNuovi deceduti\n" +
                value.totaleDeceduti + "\t\tTotale deceduti\n" + value.nuoviTamponi + "\t\tNuovi tamponi\n" + value.totaleTamponi + "\tTotale tamponi\n" +
                value.totaleGuariti + "\tTotale guariti\n" + value.totaleCasi + "\tTotale casi\n";
        }
    }
}
