using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Brushes = System.Windows.Media.Brushes;

namespace CovidItalia
{
    
    public partial class MainWindow : Window
    {
        private NotifyIcon NotifyIcon;
        private System.Windows.Forms.ContextMenu ContextMenu1;
        private System.Windows.Forms.MenuItem MenuItemExit;
        private System.Windows.Forms.MenuItem MenuItemOpen;
        private IContainer components;
        private bool f;

        public MainWindow()
        {
            try
            {
                f = false;
                components = new Container();
                ContextMenu1 = new System.Windows.Forms.ContextMenu();
                MenuItemExit = new System.Windows.Forms.MenuItem();
                MenuItemOpen = new System.Windows.Forms.MenuItem();

                ContextMenu1.MenuItems.AddRange(
                    new System.Windows.Forms.MenuItem[] { MenuItemOpen, MenuItemExit });

                MenuItemOpen.Index = 0;
                MenuItemOpen.Text = "A&pri";
                MenuItemOpen.Click += new EventHandler(Open_click);

                MenuItemExit.Index = 1;
                MenuItemExit.Text = "C&hiudi";
                MenuItemExit.Click += new EventHandler(Exit_click);

                NotifyIcon = new NotifyIcon(components);

                NotifyIcon.ContextMenu = ContextMenu1;
                NotifyIcon.Visible = true;
                NotifyIcon.Text = "Apri Aggiornamenti CoViD-19 Italia";
                NotifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);

                NotifyIcon.DoubleClick += new EventHandler(Open_click);

                ShowInTaskbar = true;

                string format = "#,##0";

                InitializeComponent();

                var values = Parser.Filtered(DateTime.Today);
                var value = values[0];

                regioni.SelectedIndex = 0;

                labels.Text = "Nuovi Contagi\n" + "Totale Positivi\n" + "Nuovi Deceduti\n" + "Totale Deceduti\n" + "Nuovi Tamponi\n" + "Totale Tamponi\n" + "Nuovi Guariti\n" + "Totale Guariti\n" + "Totale Casi";
                labelsr.Text = "Nuovi Contagi\n" + "Totale Positivi\n" + "Nuovi Deceduti\n" + "Totale Deceduti\n" + "Nuovi Tamponi\n" + "Totale Tamponi\n" + "Nuovi Guariti\n" + "Totale Guariti\n" + "Totale Casi\n" + "Regione";

                data.Text = value.data.ToString("dd MMMM yyyy");
                datar.Text = value.data.ToString("dd MMMM yyyy");
                dati.Text = value.nuoviPositivi.ToString(format) + "\n" + value.totalePositivi.ToString(format) + "\n" + value.nuoviDeceduti.ToString(format) + "\n" +
                    value.totaleDeceduti.ToString(format) + "\n" + value.nuoviTamponi.ToString(format) + "\n" + value.totaleTamponi.ToString(format) + "\n" + value.nuoviGuariti.ToString(format) + "\n" +
                    value.totaleGuariti.ToString(format) + "\n" + value.totaleCasi.ToString(format);
                datir.Text = "Nessuna regione selezionata";
                SeriesCollection = new SeriesCollection { };
                DataContext = this;

                Task.Run(() =>
                {
                    while (!f)
                    {
                        Thread.Sleep(15000);
                        NotificationService.Notification();
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void DateChanged(object sender, SelectionChangedEventArgs e)

        {
            try {
                string format = "#,##0";
                DateTime selected = (DateTime)calendar.SelectedDate;
                var values = Parser.Filtered(selected);
                var value = values[0];

                data.Text = value.data.ToString("dd MMMM yyyy");
                dati.Text = value.nuoviPositivi.ToString(format) + "\n" + value.totalePositivi.ToString(format) + "\n" + value.nuoviDeceduti.ToString(format) + "\n" +
                    value.totaleDeceduti.ToString(format) + "\n" + value.nuoviTamponi.ToString(format) + "\n" + value.totaleTamponi.ToString(format) + "\n" + value.nuoviGuariti.ToString(format) + "\n" +
                    value.totaleGuariti.ToString(format) + "\n" + value.totaleCasi.ToString(format);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                string format = "#,##0";
                ComboBoxItem cbi = (ComboBoxItem)regioni.SelectedItem;
                DateTime selected = (DateTime)calendarr.SelectedDate;
                string regione = cbi.Content.ToString();

                var valuesr = Parser.FilteredRegioni(selected, regione);
                if (regioni.SelectedIndex != 0)
                {
                    var valuer = valuesr[0];

                    datar.Text = valuer.data.ToString("dd MMMM yyyy");
                    datir.Text = valuer.nuoviPositivi.ToString(format) + "\n" + valuer.totalePositivi.ToString(format) + "\n" + valuer.nuoviDeceduti.ToString(format) + "\n" +
                        valuer.totaleDeceduti.ToString(format) + "\n" + valuer.nuoviTamponi.ToString(format) + "\n" + valuer.totaleTamponi.ToString(format) + "\n" + valuer.nuoviGuariti.ToString(format) + "\n" +
                        valuer.totaleGuariti.ToString(format) + "\n" + valuer.totaleCasi.ToString(format) + "\n" + valuer.regione;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void regioni_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            resetRegione.IsEnabled = true;
        }

        private void start_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            end.DisplayDateStart = start.SelectedDate.Value.AddDays(1);
            end.IsEnabled = true;
        }

        private void end_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            checkboxes.IsEnabled = true;
            regioniGraph.IsEnabled = true;
            reset.IsEnabled = true;
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SeriesCollection.Clear();
                ComboBoxItem cbi = (ComboBoxItem)regioniGraph.SelectedItem;
                string regione = cbi.Content.ToString();
                if (nuoviPositivi.IsChecked == true)
                {
                    SeriesCollection.Add(
                    new LineSeries
                    {
                        Title = "Nuovi Contagi",
                        Values = Parser.ChartSelector.nuoviPositivi(start.SelectedDate.Value, end.SelectedDate.Value, regione),
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
                        Values = Parser.ChartSelector.totalePositivi(start.SelectedDate.Value, end.SelectedDate.Value, regione),
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
                        Values = Parser.ChartSelector.nuoviDeceduti(start.SelectedDate.Value, end.SelectedDate.Value, regione),
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
                        Values = Parser.ChartSelector.totaleDeceduti(start.SelectedDate.Value, end.SelectedDate.Value, regione),
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
                        Values = Parser.ChartSelector.nuoviTamponi(start.SelectedDate.Value, end.SelectedDate.Value, regione),
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
                        Values = Parser.ChartSelector.totaleTamponi(start.SelectedDate.Value, end.SelectedDate.Value, regione),
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
                        Values = Parser.ChartSelector.nuoviGuariti(start.SelectedDate.Value, end.SelectedDate.Value, regione),
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
                        Values = Parser.ChartSelector.totaleGuariti(start.SelectedDate.Value, end.SelectedDate.Value, regione),
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
                        Values = Parser.ChartSelector.totaleCasi(start.SelectedDate.Value, end.SelectedDate.Value, regione),
                        LineSmoothness = 0,
                        PointGeometry = null,
                        StrokeThickness = 1,
                        Fill = Brushes.Transparent
                    });
                }
                xaxis.Labels = Parser.ChartSelector.data(start.SelectedDate.Value, end.SelectedDate.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public SeriesCollection SeriesCollection { get; set; }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            resetRegione.IsEnabled = false;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SeriesCollection.Clear();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (f == false)
            {
                e.Cancel = true;
                WindowState = WindowState.Minimized;
                Visibility = Visibility.Hidden;
                base.OnClosing(e);
            }
        }

        private void Open_click(object Sender, EventArgs e)
        {
            Visibility = Visibility.Visible;
            if (WindowState == WindowState.Minimized)
                WindowState = WindowState.Normal;
            Activate();
        }

        private void Exit_click(object Sender, EventArgs e)
        {
            f = true;
            NotifyIcon.Visible = false;
            NotifyIcon.Icon.Dispose();
            NotifyIcon.Dispose();
            Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
        }
    }
}