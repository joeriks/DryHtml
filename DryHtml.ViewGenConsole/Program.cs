
using System;
using System.IO;
using System.Linq;
namespace DryHtml.ViewGenConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var filename = (args.Length == 0) ? "viewgen.json" : args[0];

            generate(filename);

            if (args.Any(t => t == "-watch" || t == "-w"))
            {
                var f = new FileSystemWatcher(Environment.CurrentDirectory, "*.*");
                f.Changed += f_Changed;
                f.EnableRaisingEvents = true;
                Console.WriteLine("Watching " + Environment.CurrentDirectory + " for changes");
                Console.ReadLine();
            }



        }

        static void generate(string filename)
        {
            var viewGen = new DryHtml.ViewGen.ViewGenerator(filename);
            viewGen.OutputRootPath = Environment.CurrentDirectory;
            viewGen.GenerateAll();
            Console.WriteLine("Generated files");

        }

        static void f_Changed(object sender, FileSystemEventArgs e)
        {
            generate(e.FullPath);
        }
    }
}
