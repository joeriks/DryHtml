using DryHtml.SiteDiff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DryHtml.SiteSnapshotConsole
{
    class Program
    {
        public class ConfigOption
        {
            public string url { get; set; }
            public string urlList { get; set; }
            public string selector { get; set; }
            public string saveTo { get; set; }
        }

        static void Main(string[] args)
        {
            List<ConfigOption> configs;

            try
            {

                if (args.Length == 0)
                {
                    var setting = System.IO.File.ReadAllText("sitesnapshot.json");
                    configs = JsonConvert.DeserializeObject<List<ConfigOption>>(setting);
                }
                else
                {
                    configs = new List<ConfigOption>{ new ConfigOption
                    {
                        url = args[0],
                        urlList = args[1],
                        selector = args[2],
                        saveTo = args[3]
                    }};
                }


                foreach (var config in configs)
                {
                    var toPath = System.IO.Path.Combine(Environment.CurrentDirectory, config.saveTo);

                    var siteSnapshot = new SiteSnapshot(config.url, config.urlList.Split(','), config.selector, toPath);

                    Console.WriteLine("Created " + siteSnapshot.FileNames.Count().ToString() + " files:");
                    foreach (var file in siteSnapshot.FileNames)
                    {
                        Console.WriteLine(file);
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                Console.WriteLine("Params: url urls selector relativePath");
                Console.WriteLine(@"""http://xyz"" ""home,about"" ""*"" ""/site/1""");
                Console.WriteLine("or no parameter = reads sitesnapshot.json");

            }



        }
    }
}
