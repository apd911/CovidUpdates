using System;
using CovidLib;

namespace CovidCLI
{
    class CovidCLI
    {
        static void Main(string[] args)
        {
            string source = "https://raw.githubusercontent.com/pcm-dpc/COVID-19/master/dati-json/dpc-covid19-ita-andamento-nazionale.json";
            Tuple<string, int, int, int, int, int, int, Tuple<int, int>> tuple = Parser.Parse(source);
            string data = tuple.Item1;
            int npositivi = tuple.Item2;
            string pos = npositivi.ToString("#,##0");
            int tcasi = tuple.Item3;
            string casi = tcasi.ToString("#,##0");
            int gdeceduti = tuple.Item4;
            string dec = gdeceduti.ToString("#,##0");
            int gtamponi = tuple.Item5;
            string tamp = gtamponi.ToString("#,##0");
            int dimessig = tuple.Item6;
            string dimessi = dimessig.ToString("#,##0");
            int tpositivi = tuple.Item7;
            string tpos = tpositivi.ToString("#,##0");
            int deceduti = tuple.Rest.Item1;
            string tdec = deceduti.ToString("#,##0");
            int tamponi = tuple.Rest.Item2;
            string ttamp = tamponi.ToString("#,##0");

            Console.WriteLine("Aggiornamento al " + data + ".\n");
            Console.WriteLine(pos + "\t\tnuovi positivi");
            Console.WriteLine(tpos + "\t\ttotale positivi\n");
            Console.WriteLine(dec + "\t\tnuovi deceduti");
            Console.WriteLine(tdec + "\t\ttotale deceduti\n");
            Console.WriteLine(tamp + "\t\tnuovi tamponi");
            Console.WriteLine(ttamp + "\ttotale tamponi\n");
            Console.WriteLine(dimessi + "\ttotale guariti");
            Console.WriteLine(casi + "\ttotale casi");

            Console.WriteLine("\nPremere Invio per chiudere il programma...");
            Console.ReadLine();
        }
    }
}
