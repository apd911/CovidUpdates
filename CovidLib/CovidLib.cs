using CsvHelper;
using LazyCache;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;

namespace CovidLib
{
    public static class Parser
    {
        public class Dati
        {
            public string data { get; set; }
            public string regione { get; set; }
            public string nuoviPositivi { get; set; }
            public string totaleCasi { get; set; }
            public string totaleDeceduti { get; set; }
            public int totDecedutiInt { get; set; }
            public string totaleTamponi { get; set; }
            public int totTamponiInt { get; set; }
            public string totaleGuariti { get; set; }
            public string totalePositivi { get; set; }
            public string nuoviDeceduti { get; set; }
            public string nuoviTamponi { get; set; }
        }

        public static List<Dati> ReadCSV()
        {
            string source = "https://raw.githubusercontent.com/pcm-dpc/COVID-19/master/dati-andamento-nazionale/dpc-covid19-ita-andamento-nazionale.csv";
            StringReader rawFile = new StringReader(new WebClient().DownloadString(source));

            string format = "#,##0";

            using (var reader = rawFile) using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<Dati> records = new List<Dati>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    Dati regionep = new Dati();
                    regionep.totTamponiInt = 0;
                    regionep.totDecedutiInt = 0;
                    if (records.Count > 1)
                    {
                        regionep.totTamponiInt = records[records.Count - 1].totTamponiInt;
                        regionep.totDecedutiInt = records[records.Count - 1].totDecedutiInt;
                    }
                    var record = new Dati
                    {
                        data = csv.GetField<DateTime>("data").ToString("dd MMMM yyyy"),
                        regione = "italia",
                        nuoviPositivi = csv.GetField<int>("nuovi_positivi").ToString(format),
                        totaleCasi = csv.GetField<int>("totale_casi").ToString(format),
                        totaleDeceduti = csv.GetField<int>("deceduti").ToString(format),
                        totaleTamponi = csv.GetField<int>("tamponi").ToString(format),
                        totaleGuariti = csv.GetField<int>("dimessi_guariti").ToString(format),
                        totalePositivi = csv.GetField<int>("totale_positivi").ToString(format),
                        totDecedutiInt = csv.GetField<int>("deceduti"),
                        totTamponiInt = csv.GetField<int>("tamponi"),
                        nuoviDeceduti = (csv.GetField<int>("deceduti") - regionep.totDecedutiInt).ToString(format),
                        nuoviTamponi = (csv.GetField<int>("tamponi") - regionep.totTamponiInt).ToString(format)
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

            string format = "#,##0";

            using (var reader = rawFile) using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<Dati> records = new List<Dati>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    Dati regionep = new Dati();
                    regionep.totTamponiInt = 0;
                    regionep.totDecedutiInt = 0;
                    if (records.Count > 21)
                    {
                        regionep.totTamponiInt = records[records.Count - 21].totTamponiInt;
                        regionep.totDecedutiInt = records[records.Count - 21].totDecedutiInt;
                    }
                    
                    var record = new Dati
                    {
                        data = csv.GetField<DateTime>("data").ToString("dd MMMM yyyy"),
                        regione = csv.GetField("denominazione_regione").ToString(),
                        nuoviPositivi = csv.GetField<int>("nuovi_positivi").ToString(format),
                        totaleCasi = csv.GetField<int>("totale_casi").ToString(format),
                        totaleDeceduti = csv.GetField<int>("deceduti").ToString(format),
                        totaleTamponi = csv.GetField<int>("tamponi").ToString(format),
                        totDecedutiInt = csv.GetField<int>("deceduti"),
                        totTamponiInt = csv.GetField<int>("tamponi"),
                        totaleGuariti = csv.GetField<int>("dimessi_guariti").ToString(format),
                        totalePositivi = csv.GetField<int>("totale_positivi").ToString(format),
                        nuoviDeceduti = (csv.GetField<int>("deceduti") - regionep.totDecedutiInt).ToString(format),
                        nuoviTamponi = (csv.GetField<int>("tamponi") - regionep.totTamponiInt).ToString(format)
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

                if (record.data == date.ToString("dd MMMM yyyy"))
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

                if (record.data == date.ToString("dd MMMM yyyy"))
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
    }
}
