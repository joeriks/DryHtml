using CsQuery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DryHtml.SiteDiff
{
    public static class Utilities
    {

        public static CQ CQ(string pathOrUrl)
        {
            return new CQ(GetHtmlFromFileOrUrl(pathOrUrl));
        }
        public static string GetFullPath(string pathOrUrl, string fileOrPath = "")
        {
            if (pathOrUrl.StartsWith("http"))
            {
                if (!pathOrUrl.EndsWith("/")) pathOrUrl += "/";
                return pathOrUrl + fileOrPath;
            }
            return Path.Combine(pathOrUrl, fileOrPath);
        }

        public static string GetHtmlFromFileOrUrl(string fullPathOrUrl)
        {

            var result = "";
            if (fullPathOrUrl.StartsWith("http"))
            {
                using (var webClient = new WebClient())
                {
                    result = webClient.DownloadString(fullPathOrUrl);
                }
            }
            else
            {
                result = System.IO.File.ReadAllText(fullPathOrUrl);
            }
            return result;
        }
    }
}
