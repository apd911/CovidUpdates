using System;
using CovidLib;

namespace CovidCLI
{
    class CovidCLI
    {
        static void Main(string[] args)
        {
            //var records = Parser.ReadCSV();
            //var record = records[records.Count - 1];

            var records = Parser.Filtered(DateTime.Today.AddDays(-5));
            var record = records[0];
            
            Console.WriteLine("Aggiornamento: " + record.data + ".\n");
            Console.WriteLine(record.nuoviPositivi + "\t\tnuovi positivi");
            Console.WriteLine(record.totalePositivi + "\t\ttotale positivi\n");
            Console.WriteLine(record.nuoviDeceduti + "\t\tnuovi deceduti");
            Console.WriteLine(record.totaleDeceduti + "\t\ttotale deceduti\n");
            Console.WriteLine(record.nuoviTamponi + "\t\tnuovi tamponi");
            Console.WriteLine(record.totaleTamponi + "\ttotale tamponi\n");
            Console.WriteLine(record.totaleGuariti + "\ttotale guariti");
            Console.WriteLine(record.totaleCasi + "\ttotale casi\n");

            Console.WriteLine("\nPremere Invio per chiudere il programma...");
            Console.ReadLine();
        }
    }
}
