using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryHtml.SiteDiffConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var argsDictionary = args.Where(t => t.Contains('=')).ToDictionary(t => t.Split('=')[0], t=> t.Split('=')[1]);

            var argsList = args.Where(t => !t.Contains('=')).ToArray();

            try
            {
                var url1 = argsList[0]; // argsDictionary["url1"] 
                var url2 = argsList[1]; // argsDictionary["url2"]
                var selector = argsList[2]; // argsDictionary["selector"]
                var excludeSelectors = argsList[3].Split(','); // argsDictionary["selector"]
                var urls = argsList[4].Split(','); // argsDictionary["urls"]
                try
                {
                    var compare = new DryHtml.SiteDiff.SiteCompare(url1, url2, urls, selector, excludeSelectors);
                    Console.WriteLine(compare.TextReport);

                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine("Error " + ex.ToString());
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error. Expected input: url1 url2 selector excludeselector(s) urls.");
                Console.WriteLine(@"Example ""http://xyz"" ""http://zzz"" ""body"" ""#random,#time"" ""page1,page2,page3,page1/page11""");

            }                        

        }
    }
}
