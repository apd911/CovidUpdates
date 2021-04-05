using System;
using System.Text.Json;
using System.Net;

namespace CovidLib
{
    public struct Values
    {
        public string data;
        public string nuoviPositivi;
        public string totaleCasi;
        public string nuoviDeceduti;
        public string totaleDeceduti;
        public string nuoviTamponi;
        public string totaleTamponi;
        public string totaleGuariti;
        public string totalePositivi;
    }

    public struct RawValues
    {
        public string data;
        public int nuoviPositivi;
        public int totaleCasi;
        public int totaleDeceduti;
        public int totaleDecedutiPrecedenti;
        public int totaleTamponi;
        public int totaleTamponiPrecedenti;
        public int totaleGuariti;
        public int totalePositivi;
        public int nuoviDeceduti;
        public int nuoviTamponi;
    }
    public static class Parser
    {
        

        public static RawValues Parse(string source)
        {
            RawValues rawValue = new RawValues();

            var json = new WebClient().DownloadString(source);
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            int last = root.GetArrayLength() - 1;
            int ptolast = root.GetArrayLength() - 2;

            var u1 = root[last];
            var u2 = root[ptolast];

            rawValue.data = u1.GetProperty("data").GetDateTime().ToString("dd MMMM yyyy");
            rawValue.nuoviPositivi = u1.GetProperty("nuovi_positivi").GetInt32();
            rawValue.totaleCasi = u1.GetProperty("totale_casi").GetInt32();
            rawValue.totaleDeceduti = u1.GetProperty("deceduti").GetInt32();
            rawValue.totaleDecedutiPrecedenti = u2.GetProperty("deceduti").GetInt32();
            rawValue.totaleTamponi = u1.GetProperty("tamponi").GetInt32();
            rawValue.totaleTamponiPrecedenti = u2.GetProperty("tamponi").GetInt32();
            rawValue.totaleGuariti = u1.GetProperty("dimessi_guariti").GetInt32();
            rawValue.totalePositivi = u1.GetProperty("totale_positivi").GetInt32();

            rawValue.nuoviDeceduti = rawValue.totaleDeceduti - rawValue.totaleDecedutiPrecedenti;
            rawValue.nuoviTamponi = rawValue.totaleTamponi - rawValue.totaleTamponiPrecedenti;

            return rawValue;
        }

        public static Values Format(string source)
        {
            RawValues raw = Parse(source);

            Values V = new Values();

            string format = "#,##0";

            V.data = raw.data;
            V.nuoviPositivi = raw.nuoviPositivi.ToString(format);
            V.totaleCasi = raw.totaleCasi.ToString(format);
            V.nuoviDeceduti = raw.nuoviDeceduti.ToString(format);
            V.totaleDeceduti = raw.totaleDeceduti.ToString(format);
            V.nuoviTamponi = raw.nuoviTamponi.ToString(format);
            V.totaleTamponi = raw.totaleTamponi.ToString(format);
            V.totaleGuariti = raw.totaleGuariti.ToString(format);
            V.totalePositivi = raw.totalePositivi.ToString(format);

            return V;
        }
    }
}
