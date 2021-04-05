using System;
using CovidLib;

namespace CovidCLI
{
    class CovidCLI
    {
        static void Main(string[] args)
        {
            string source = "https://raw.githubusercontent.com/pcm-dpc/COVID-19/master/dati-json/dpc-covid19-ita-andamento-nazionale.json";
            Values value = Parser.Format(source);
            
            Console.WriteLine("Aggiornamento al " + value.data + ".\n");
            Console.WriteLine(value.nuoviPositivi + "\t\tnuovi positivi");
            Console.WriteLine(value.totalePositivi + "\t\ttotale positivi\n");
            Console.WriteLine(value.nuoviDeceduti + "\t\tnuovi deceduti");
            Console.WriteLine(value.totaleDeceduti + "\t\ttotale deceduti\n");
            Console.WriteLine(value.nuoviTamponi + "\t\tnuovi tamponi");
            Console.WriteLine(value.totaleTamponi + "\ttotale tamponi\n");
            Console.WriteLine(value.totaleGuariti + "\ttotale guariti");
            Console.WriteLine(value.totaleCasi + "\ttotale casi");

            Console.WriteLine("\nPremere Invio per chiudere il programma...");
            Console.ReadLine();
        }
    }
}
