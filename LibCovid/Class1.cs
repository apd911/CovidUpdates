using CsvHelper;
using LazyCache;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Configurations;

namespace LibCovid
{
    public class DateModel
    {
        public System.DateTime DateTime { get; set; }
        public double Value { get; set; }
    }

    public static class Parser
    {
        public class Dati
        {
            public DateTime data { get; set; }
            public string regione { get; set; }
            public int nuoviPositivi { get; set; }
            public int totaleCasi { get; set; }
            public int totaleDeceduti { get; set; }
            public int totaleTamponi { get; set; }
            public int totaleGuariti { get; set; }
            public int totalePositivi { get; set; }
            public int nuoviDeceduti { get; set; }
            public int nuoviTamponi { get; set; }
        }

        public static List<Dati> ReadCSV()
        {
            string source = "https://raw.githubusercontent.com/pcm-dpc/COVID-19/master/dati-andamento-nazionale/dpc-covid19-ita-andamento-nazionale.csv";
            StringReader rawFile = new StringReader(new WebClient().DownloadString(source));

            using (var reader = rawFile) using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<Dati> records = new List<Dati>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    Dati regionep = new Dati();
                    regionep.totaleTamponi = 0;
                    regionep.totaleDeceduti = 0;
                    if (records.Count > 1)
                    {
                        regionep.totaleTamponi = records[records.Count - 1].totaleTamponi;
                        regionep.totaleDeceduti = records[records.Count - 1].totaleDeceduti;
                    }
                    var record = new Dati
                    {
                        data = csv.GetField<DateTime>("data"),
                        regione = "italia",
                        nuoviPositivi = csv.GetField<int>("nuovi_positivi"),
                        totaleCasi = csv.GetField<int>("totale_casi"),
                        totaleDeceduti = csv.GetField<int>("deceduti"),
                        totaleTamponi = csv.GetField<int>("tamponi"),
                        totaleGuariti = csv.GetField<int>("dimessi_guariti"),
                        totalePositivi = csv.GetField<int>("totale_positivi"),
                        nuoviDeceduti = (csv.GetField<int>("deceduti") - regionep.totaleDeceduti),
                        nuoviTamponi = (csv.GetField<int>("tamponi") - regionep.totaleTamponi)
                    };

                    records.Add(record);
                }

                return records;
            }
        }

        public static List<Dati> ReadCSVRegioni()
        {
            string source = "https://raw.githubusercontent.com/pcm-dpc/COVID-19/master/dati-regioni/dpc-covid19-ita-regioni.csv";
            StringReader rawFile = new StringReader(new WebClient().DownloadString(source));

            using (var reader = rawFile) using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<Dati> records = new List<Dati>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    Dati regionep = new Dati();
                    regionep.totaleTamponi = 0;
                    regionep.totaleDeceduti = 0;
                    if (records.Count > 21)
                    {
                        regionep.totaleTamponi = records[records.Count - 21].totaleTamponi;
                        regionep.totaleDeceduti = records[records.Count - 21].totaleDeceduti;
                    }

                    var record = new Dati
                    {
                        data = csv.GetField<DateTime>("data"),
                        regione = csv.GetField("denominazione_regione").ToString(),
                        nuoviPositivi = csv.GetField<int>("nuovi_positivi"),
                        totaleCasi = csv.GetField<int>("totale_casi"),
                        totaleDeceduti = csv.GetField<int>("deceduti"),
                        totaleTamponi = csv.GetField<int>("tamponi"),
                        totaleGuariti = csv.GetField<int>("dimessi_guariti"),
                        totalePositivi = csv.GetField<int>("totale_positivi"),
                        nuoviDeceduti = (csv.GetField<int>("deceduti") - regionep.totaleDeceduti),
                        nuoviTamponi = (csv.GetField<int>("tamponi") - regionep.totaleTamponi)
                    };

                    records.Add(record);
                }

                return records;
            }
        }

        public static List<Dati> Filtered(DateTime date)
        {
            List<Dati> filtered = new List<Dati>();

            IAppCache cache = new CachingService();

            Func<List<Dati>> getData = () => ReadCSV();

            var records = cache.GetOrAdd("italia.Get", getData);

            foreach (var record in records)
            {
                var filter = records[records.Count - 1];
                filtered.Add(filter);

                if (record.data.ToString("dd MMMM yyyy") == date.ToString("dd MMMM yyyy"))
                {
                    filter = record;
                    filtered.Clear();
                    filtered.Add(filter);
                }
            }

            return filtered;
        }

        public static List<Dati> FilteredRegioni(DateTime date, string regione)
        {
            List<Dati> filtered = new List<Dati>();

            IAppCache cache = new CachingService();

            Func<List<Dati>> getData = () => ReadCSVRegioni();

            var records = cache.GetOrAdd("regioni.Get", getData);

            foreach (var record in records)
            {
                var filter = records[records.Count - 1];
                filtered.Add(filter);

                if (record.data.ToString("dd MMMM yyyy") == date.ToString("dd MMMM yyyy"))
                {
                    if (record.regione == regione)
                    {
                        filter = record;
                        filtered.Clear();
                        filtered.Add(filter);
                    }
                }
            }

            return filtered;
        }

        public class ChartSelector
        {
            public static ChartValues<int> nuoviPositivi(DateTime start, DateTime end)
            {
                ChartValues<int> filtered = new ChartValues<int>();
                IAppCache cache = new CachingService();
                Func<List<Dati>> getData = () => ReadCSV();
                var records = cache.GetOrAdd("italia.Get", getData);

                foreach (var record in records)
                {
                    var filter = records[records.Count - 1];

                    if (DateTime.Compare(record.data, start) == 0 | DateTime.Compare(record.data, start) == 1)
                    {
                        if (DateTime.Compare(record.data, end) == 0 | DateTime.Compare(record.data, end) == -1)
                        {
                            filtered.Add(record.nuoviPositivi);
                        }
                    }
                }
                return filtered;
            }

            public static ChartValues<int> nuoviDeceduti(DateTime start, DateTime end)
            {
                ChartValues<int> filtered = new ChartValues<int>();
                IAppCache cache = new CachingService();
                Func<List<Dati>> getData = () => ReadCSV();
                var records = cache.GetOrAdd("italia.Get", getData);

                foreach (var record in records)
                {
                    var filter = records[records.Count - 1];

                    if (DateTime.Compare(record.data, start) == 0 | DateTime.Compare(record.data, start) == 1)
                    {
                        if (DateTime.Compare(record.data, end) == 0 | DateTime.Compare(record.data, end) == -1)
                        {
                            filtered.Add(record.nuoviDeceduti);
                        }
                    }
                }
                return filtered;
            }

            public static ChartValues<int> totalePositivi(DateTime start, DateTime end)
            {
                ChartValues<int> filtered = new ChartValues<int>();
                IAppCache cache = new CachingService();
                Func<List<Dati>> getData = () => ReadCSV();
                var records = cache.GetOrAdd("italia.Get", getData);

                foreach (var record in records)
                {
                    var filter = records[records.Count - 1];

                    if (DateTime.Compare(record.data, start) == 0 | DateTime.Compare(record.data, start) == 1)
                    {
                        if (DateTime.Compare(record.data, end) == 0 | DateTime.Compare(record.data, end) == -1)
                        {
                            filtered.Add(record.totalePositivi);
                        }
                    }
                }
                return filtered;
            }

            public static ChartValues<int> totaleDeceduti(DateTime start, DateTime end)
            {
                ChartValues<int> filtered = new ChartValues<int>();
                IAppCache cache = new CachingService();
                Func<List<Dati>> getData = () => ReadCSV();
                var records = cache.GetOrAdd("italia.Get", getData);

                foreach (var record in records)
                {
                    var filter = records[records.Count - 1];

                    if (DateTime.Compare(record.data, start) == 0 | DateTime.Compare(record.data, start) == 1)
                    {
                        if (DateTime.Compare(record.data, end) == 0 | DateTime.Compare(record.data, end) == -1)
                        {
                            filtered.Add(record.totaleDeceduti);
                        }
                    }
                }
                return filtered;
            }

            public static ChartValues<int> nuoviTamponi(DateTime start, DateTime end)
            {
                ChartValues<int> filtered = new ChartValues<int>();
                IAppCache cache = new CachingService();
                Func<List<Dati>> getData = () => ReadCSV();
                var records = cache.GetOrAdd("italia.Get", getData);

                foreach (var record in records)
                {
                    var filter = records[records.Count - 1];

                    if (DateTime.Compare(record.data, start) == 0 | DateTime.Compare(record.data, start) == 1)
                    {
                        if (DateTime.Compare(record.data, end) == 0 | DateTime.Compare(record.data, end) == -1)
                        {
                            filtered.Add(record.nuoviTamponi);
                        }
                    }
                }
                return filtered;
            }

            public static ChartValues<int> totaleTamponi(DateTime start, DateTime end)
            {
                ChartValues<int> filtered = new ChartValues<int>();
                IAppCache cache = new CachingService();
                Func<List<Dati>> getData = () => ReadCSV();
                var records = cache.GetOrAdd("italia.Get", getData);

                foreach (var record in records)
                {
                    var filter = records[records.Count - 1];

                    if (DateTime.Compare(record.data, start) == 0 | DateTime.Compare(record.data, start) == 1)
                    {
                        if (DateTime.Compare(record.data, end) == 0 | DateTime.Compare(record.data, end) == -1)
                        {
                            filtered.Add(record.totaleTamponi);
                        }
                    }
                }
                return filtered;
            }

            public static ChartValues<int> totaleGuariti(DateTime start, DateTime end)
            {
                ChartValues<int> filtered = new ChartValues<int>();
                IAppCache cache = new CachingService();
                Func<List<Dati>> getData = () => ReadCSV();
                var records = cache.GetOrAdd("italia.Get", getData);

                foreach (var record in records)
                {
                    var filter = records[records.Count - 1];

                    if (DateTime.Compare(record.data, start) == 0 | DateTime.Compare(record.data, start) == 1)
                    {
                        if (DateTime.Compare(record.data, end) == 0 | DateTime.Compare(record.data, end) == -1)
                        {
                            filtered.Add(record.totaleGuariti);
                        }
                    }
                }
                return filtered;
            }

            public static ChartValues<int> totaleCasi(DateTime start, DateTime end)
            {
                ChartValues<int> filtered = new ChartValues<int>();
                IAppCache cache = new CachingService();
                Func<List<Dati>> getData = () => ReadCSV();
                var records = cache.GetOrAdd("italia.Get", getData);

                foreach (var record in records)
                {
                    var filter = records[records.Count - 1];

                    if (DateTime.Compare(record.data, start) == 0 | DateTime.Compare(record.data, start) == 1)
                    {
                        if (DateTime.Compare(record.data, end) == 0 | DateTime.Compare(record.data, end) == -1)
                        {
                            filtered.Add(record.totaleCasi);
                        }
                    }
                }
                return filtered;
            }

            public static ChartValues<string> data(DateTime start, DateTime end)
            {
                ChartValues<string> filtered = new ChartValues<string>();
                IAppCache cache = new CachingService();
                Func<List<Dati>> getData = () => ReadCSV();
                var records = cache.GetOrAdd("italia.Get", getData);

                foreach (var record in records)
                {
                    var filter = records[records.Count - 1];

                    if (DateTime.Compare(record.data, start) == 0 | DateTime.Compare(record.data, start) == 1)
                    {
                        if (DateTime.Compare(record.data, end) == 0 | DateTime.Compare(record.data, end) == -1)
                        {
                            filtered.Add(record.data.ToString("dd MMM"));
                        }
                    }
                }
                return filtered;
            }
        }
    }
}
