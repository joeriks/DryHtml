
using System;
namespace DryHtml.ViewGenConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var filename = (args.Length == 0) ? "viewgen.json" : args[0];

            var viewGen = new DryHtml.ViewGen.ViewGenerator(filename);

            viewGen.OutputRootPath = Environment.CurrentDirectory;

            viewGen.GenerateAll();

            Console.WriteLine("Done");

        }
    }
}
