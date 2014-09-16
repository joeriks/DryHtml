using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace DryHtml.SiteDiff
{
    public class SiteSnapshot
    {
        private string rootUrl;
        private string[] urlList;
        private string selector;
        private string dumpToPath;

        public SiteSnapshot(string rootUrl, string[] urlList, string selector, string dumpToPath)
        {
            // TODO: Complete member initialization
            this.rootUrl = rootUrl;
            this.urlList = urlList;
            this.selector = selector;
            this.dumpToPath = dumpToPath;

            foreach (var url in urlList)
            {

                using (WebClient client = new WebClient())
                {
                    string s = client.DownloadString(rootUrl + url);
                    var saveToPath = System.IO.Path.Combine(dumpToPath, url) + ".html";
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(saveToPath));
                    System.IO.File.WriteAllText(saveToPath, s);
                }

            }

        }
    }
}
