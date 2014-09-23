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
            public string siteMapUrl { get; set; }
            public string siteMapSelector { get; set; }
        }

        static void Main(string[] args)
        {
            var configs = new List<ConfigOption>();
            var defaultFilename = "sitesnapshot.json";
            try
            {

                if (args.Length == 0 && System.IO.File.Exists(defaultFilename))
                {
                    var setting = System.IO.File.ReadAllText(defaultFilename);
                    configs = JsonConvert.DeserializeObject<List<ConfigOption>>(setting);
                }
                else
                {
                    if (args.Length == 4)
                    {
                        configs = new List<ConfigOption>{ new ConfigOption
                    {
                        url = args[0],
                        urlList = args[1],
                        selector = args[2],
                        saveTo = args[3]
                    }};
                    }

                    else if (args.Length == 5)
                    {
                        configs = new List<ConfigOption>{ new ConfigOption
                    {
                        url = args[0],
                        selector = args[1],
                        siteMapUrl = args[2],
                        siteMapSelector = args[3],
                        saveTo = args[4]
                    }};

                    }

                    if (configs.Count == 0) usage();
                    else
                        foreach (var config in configs)
                        {
                            var toPath = System.IO.Path.Combine(Environment.CurrentDirectory, config.saveTo);

                            var siteSnapshot = string.IsNullOrEmpty(config.siteMapUrl) ?
                                new SiteSnapshot(config.url, config.urlList.Split(','), config.selector, toPath)
                                : new SiteSnapshot(config.url, config.selector, config.siteMapUrl, config.siteMapSelector, toPath);

                            Console.WriteLine("Created " + siteSnapshot.FileNames.Count().ToString() + " files:");
                            foreach (var file in siteSnapshot.FileNames)
                            {
                                Console.WriteLine(file);
                            }
                        }
                } 

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
                Console.WriteLine(ex.ToString());
                usage();
            }

        }
        private static void usage()
        {

            Console.WriteLine("Params: url urls selector relativePath");
            Console.WriteLine(@"""http://xyz"" ""home,about"" ""*"" ""/site/1""");
            Console.WriteLine("or");
            Console.WriteLine("No parameter = reads sitesnapshot.json");
            Console.WriteLine("or");
            Console.WriteLine("Params: url selector urlToSiteMap selectorForSitemap relativePath");
            Console.WriteLine(@"""http://xyz"" ""*"" ""http://xyz/sitemap"" ""ul.sitemap"" ""/site/1""");

        }

    }
}
