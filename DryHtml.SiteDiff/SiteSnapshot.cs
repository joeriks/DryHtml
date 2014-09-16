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

        public List<string> FileNames { get; set; }

        public SiteSnapshot(string rootUrl, string[] urlList, string selector, string dumpToPath)
        {
            this.rootUrl = rootUrl;
            this.urlList = urlList;
            this.selector = selector;
            this.dumpToPath = dumpToPath;

            FileNames = new List<string>();

            foreach (var url in urlList)
            {

                var html = DryHtml.SiteDiff.Utilities.CQ(rootUrl + url.Trim(), true);
                var result = "";
                if (!string.IsNullOrEmpty(selector) && selector != "*")
                    result = html.Select(selector).RenderSelection();
                else
                    result = html.Render();

                result = System.Net.WebUtility.HtmlDecode(result);

                var saveToPath = System.IO.Path.Combine(dumpToPath + url) + ".html";
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(saveToPath));
                System.IO.File.WriteAllText(saveToPath, result,Encoding.Unicode);
                FileNames.Add(saveToPath);

            }

                
        }
    }
}
