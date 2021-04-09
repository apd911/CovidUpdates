using System.Net;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System;
using CsvHelper;

namespace CovidLib
{
    public class Dati
    {
        public string data{ get; set; }
        public string nuoviPositivi{ get; set; }
        public string totaleCasi{ get; set; }
        public string totaleDeceduti{ get; set; }
        public string totaleTamponi{ get; set; }
        public string totaleGuariti{ get; set; }
        public string totalePositivi{ get; set; }
        public string nuoviDeceduti{ get; set; }
        public string nuoviTamponi{ get; set; }
    }

    public static class Parser
    {
        public static List<Dati> ReadCSV()
        {
            string source = "https://raw.githubusercontent.com/pcm-dpc/COVID-19/master/dati-andamento-nazionale/dpc-covid19-ita-andamento-nazionale.csv";
            StringReader rawFile = new StringReader(new WebClient().DownloadString(source));

            int decedutiPrecedenti = 0;
            int tamponiPrecedenti = 0;

            string format = "#,##0";

            using (var reader = rawFile) using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<Dati> records = new List<Dati>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new Dati
                    {
                        data = csv.GetField<DateTime>("data").ToString("dd MMMM yyyy"),
                        nuoviPositivi = csv.GetField<int>("nuovi_positivi").ToString(format),
                        totaleCasi = csv.GetField<int>("totale_casi").ToString(format),
                        totaleDeceduti = csv.GetField<int>("deceduti").ToString(format),
                        totaleTamponi = csv.GetField<int>("tamponi").ToString(format),
                        totaleGuariti = csv.GetField<int>("dimessi_guariti").ToString(format),
                        totalePositivi = csv.GetField<int>("totale_positivi").ToString(format),
                        nuoviDeceduti = (csv.GetField<int>("deceduti") - decedutiPrecedenti).ToString(format),
                        nuoviTamponi = (csv.GetField<int>("tamponi") - tamponiPrecedenti).ToString(format)
                    };
                    decedutiPrecedenti = csv.GetField<int>("deceduti");
                    tamponiPrecedenti = csv.GetField<int>("tamponi");

                    records.Add(record);
                }

                return records;
            }
        }

        public static List<Dati> Filtered(DateTime date)
        {
            List<Dati> filtered = new List<Dati>();

            List<Dati> records = ReadCSV();

            foreach (var record in records )
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
    }
}
