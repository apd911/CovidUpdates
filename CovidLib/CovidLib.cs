using System;
using System.Text.Json;
using System.Net;

namespace CovidLib
{
    public static class Parser
    {
        public static Tuple<string, int, int, int, int, int, int, Tuple<int, int>> Parse(string source)
        {
            var json = new WebClient().DownloadString(source);
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            int last = root.GetArrayLength() - 1;
            int ptolast = root.GetArrayLength() - 2;

            var u1 = root[last];
            var u2 = root[ptolast];

            DateTime datar = u1.GetProperty("data").GetDateTime();
            string data = datar.ToString("dd MMMM yyyy");
            int npositivi = u1.GetProperty("nuovi_positivi").GetInt32();
            int tcasi = u1.GetProperty("totale_casi").GetInt32();
            int deceduti = u1.GetProperty("deceduti").GetInt32();
            int pdeceduti = u2.GetProperty("deceduti").GetInt32();
            int tamponi = u1.GetProperty("tamponi").GetInt32();
            int ptamponi = u2.GetProperty("tamponi").GetInt32();
            int dimessig = u1.GetProperty("dimessi_guariti").GetInt32();
            int tpositivi = u1.GetProperty("totale_positivi").GetInt32();

            int gdeceduti = deceduti - pdeceduti;
            int gtamponi = tamponi - ptamponi;


            return new Tuple<string, int, int, int, int, int, int, Tuple<int, int>>(data, npositivi, tcasi, gdeceduti, gtamponi, dimessig, tpositivi, new Tuple<int, int>(deceduti, tamponi));
        }
    }
}
