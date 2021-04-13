using System;
using System.Windows;
using System.Windows.Controls;
using LibCovid;
using LiveCharts;
using LiveCharts.Wpf;
using Brushes = System.Windows.Media.Brushes;

namespace CovidItalia
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            string format = "#,##0";

            InitializeComponent();

            var values = Parser.Filtered(DateTime.Today);
            var value = values[0];

            regioni.Text = "-- Seleziona --";

            labels.Text = "Nuovi Contagi\n" + "Totale Positivi\n" + "Nuovi Deceduti\n" + "Totale Deceduti\n" + "Nuovi Tamponi\n" + "Totale Tamponi\n" + "Nuovi Guariti\n" + "Totale Guariti\n" + "Totale Casi";
            labelsr.Text = "Nuovi Contagi\n" + "Totale Positivi\n" + "Nuovi Deceduti\n" + "Totale Deceduti\n" + "Nuovi Tamponi\n" + "Totale Tamponi\n" + "Nuovi Guariti\n" + "Totale Guariti\n" + "Totale Casi\n" + "Regione";

            data.Text = value.data.ToString("dd MMMM yyyy");
            datar.Text = value.data.ToString("dd MMMM yyyy");
            dati.Text = value.nuoviPositivi.ToString(format) + "\n" + value.totalePositivi.ToString(format) + "\n" + value.nuoviDeceduti.ToString(format) + "\n" +
                value.totaleDeceduti.ToString(format) + "\n" + value.nuoviTamponi.ToString(format) + "\n" + value.totaleTamponi.ToString(format) + "\n" + value.nuoviGuariti.ToString(format) + "\n" +
                value.totaleGuariti.ToString(format) + "\n" + value.totaleCasi.ToString(format);
            datir.Text = value.nuoviPositivi.ToString(format) + "\n" + value.totalePositivi.ToString(format) + "\n" + value.nuoviDeceduti.ToString(format) + "\n" +
                value.totaleDeceduti.ToString(format) + "\n" + value.nuoviTamponi.ToString(format) + "\n" + value.totaleTamponi.ToString(format) + "\n" + value.nuoviGuariti.ToString(format) + "\n" +
                value.totaleGuariti.ToString(format) + "\n" + value.totaleCasi.ToString(format) + "\nItalia";
            SeriesCollection = new SeriesCollection{};
            DataContext = this;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();

            base.OnStateChanged(e);
        }

        private void DateChanged(object sender, SelectionChangedEventArgs e)

        {
            string format = "#,##0";
            DateTime selected = (DateTime)calendar.SelectedDate;
            var values = Parser.Filtered(selected);
            var value = values[0];

            data.Text = value.data.ToString("dd MMMM yyyy");
            dati.Text = value.nuoviPositivi.ToString(format) + "\n" + value.totalePositivi.ToString(format) + "\n" + value.nuoviDeceduti.ToString(format) + "\n" +
                value.totaleDeceduti.ToString(format) + "\n" + value.nuoviTamponi.ToString(format) + "\n" + value.totaleTamponi.ToString(format) + "\n" + value.nuoviGuariti.ToString(format) + "\n" +
                value.totaleGuariti.ToString(format) + "\n" + value.totaleCasi.ToString(format);
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string format = "#,##0";
            ComboBoxItem cbi = (ComboBoxItem)regioni.SelectedItem;
            DateTime selected = (DateTime)calendarr.SelectedDate;
            string regione = "Abruzzo";
            if (cbi != null) { regione = cbi.Content.ToString(); }

            var valuesr = Parser.FilteredRegioni(selected, regione);
            var valuer = valuesr[0];

            datar.Text = valuer.data.ToString("dd MMMM yyyy");
            datir.Text = valuer.nuoviPositivi.ToString(format) + "\n" + valuer.totalePositivi.ToString(format) + "\n" + valuer.nuoviDeceduti.ToString(format) + "\n" +
               valuer.totaleDeceduti.ToString(format) + "\n" + valuer.nuoviTamponi.ToString(format) + "\n" + valuer.totaleTamponi.ToString(format) + "\n" + valuer.nuoviGuariti.ToString(format) + "\n" +
               valuer.totaleGuariti.ToString(format) + "\n" + valuer.totaleCasi.ToString(format) + "\n" + valuer.regione;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> XFormatter { get; set; }

        private void start_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            end.DisplayDateStart = start.SelectedDate.Value.AddDays(1);
            end.IsEnabled = true;
        }

        private void end_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            checkboxes.IsEnabled = true;
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            SeriesCollection.Clear();
            if (nuoviPositivi.IsChecked == true)
            {
                SeriesCollection.Add(
                new LineSeries
                {
                    Title = "Nuovi Contagi",
                    Values = Parser.ChartSelector.nuoviPositivi(start.SelectedDate.Value, end.SelectedDate.Value),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent
                });
            }

            if (totalePositivi.IsChecked == true)
            {
                SeriesCollection.Add(
                new LineSeries
                {
                    Title = "Totale Positivi",
                    Values = Parser.ChartSelector.totalePositivi(start.SelectedDate.Value, end.SelectedDate.Value),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent
                });
            }

            if (nuoviDeceduti.IsChecked == true)
            {
                SeriesCollection.Add(
                new LineSeries
                {
                    Title = "Nuovi Deceduti",
                    Values = Parser.ChartSelector.nuoviDeceduti(start.SelectedDate.Value, end.SelectedDate.Value),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent
                });
            }

            if (totaleDeceduti.IsChecked == true)
            {
                SeriesCollection.Add(
                new LineSeries
                {
                    Title = "Totale Deceduti",
                    Values = Parser.ChartSelector.totaleDeceduti(start.SelectedDate.Value, end.SelectedDate.Value),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent
                });
            }

            if (nuoviTamponi.IsChecked == true)
            {
                SeriesCollection.Add(
                new LineSeries
                {
                    Title = "Nuovi Tamponi",
                    Values = Parser.ChartSelector.nuoviTamponi(start.SelectedDate.Value, end.SelectedDate.Value),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent
                });
            }

            if (totaleTamponi.IsChecked == true)
            {
                SeriesCollection.Add(
                new LineSeries
                {
                    Title = "Totale Tamponi",
                    Values = Parser.ChartSelector.totaleTamponi(start.SelectedDate.Value, end.SelectedDate.Value),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent
                });
            }

            if (nuoviGuariti.IsChecked == true)
            {
                SeriesCollection.Add(
                new LineSeries
                {
                    Title = "Nuovi Guariti",
                    Values = Parser.ChartSelector.nuoviGuariti(start.SelectedDate.Value, end.SelectedDate.Value),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent
                });
            }

            if (totaleGuariti.IsChecked == true)
            {
                SeriesCollection.Add(
                new LineSeries
                {
                    Title = "Totale Guariti",
                    Values = Parser.ChartSelector.totaleGuariti(start.SelectedDate.Value, end.SelectedDate.Value),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent
                });
            }

            if (totaleCasi.IsChecked == true)
            {
                SeriesCollection.Add(
                new LineSeries
                {
                    Title = "Totale Casi",
                    Values = Parser.ChartSelector.totaleCasi(start.SelectedDate.Value, end.SelectedDate.Value),
                    LineSmoothness = 0,
                    PointGeometry = null,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent
                });
                
            }
            xaxis.Labels = Parser.ChartSelector.data(start.SelectedDate.Value, end.SelectedDate.Value);
        }
    }
}
