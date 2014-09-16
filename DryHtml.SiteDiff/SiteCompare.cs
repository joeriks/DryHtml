using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DryHtml.SiteDiff
{
    public class SiteCompare
    {
        private string comparePath1;
        private string comparePath2;
        private string[] urlList;
        private string selector;

        public List<SiteCompareResult> CompareResult { get; set; }

        public class SiteCompareResult
        {
            public string path1 { get; set; }
            public string path2 { get; set; }            
            public List<HtmlComparer.Diff> Diffs { get; set; }
        }

        private string textReport;
        private string[] excludeSelectors;

        public string TextReport
        {
            get
            {
                if (string.IsNullOrEmpty(textReport)) textReport = generateTextReport();
                return textReport;
            }
        }
        private string generateTextReport()
        {

            var report = new StringBuilder();
            var totalDiffs = 0;
            report.AppendLine("=======================");
            report.AppendLine("Compare:");
            report.AppendLine("Site 1: " + comparePath1);
            report.AppendLine("Site 2: " + comparePath1);
            report.AppendLine("Selector: " + selector);
            if (excludeSelectors!=null)
            report.AppendLine("Exclude selectors: " + string.Join(",",excludeSelectors));            
            report.AppendLine("=======================");
            foreach (var result in CompareResult)
            {
                report.AppendLine("Path 1: " + result.path1);
                report.AppendLine("Path 2: " + result.path2);

                totalDiffs += result.Diffs.Count;

                if (result.Diffs.Count == 0)
                {
                    report.AppendLine("Equals");
                }
                else
                {
                    report.AppendLine("Number of diffs: " + result.Diffs.Count.ToString());
                    foreach (var diff in result.Diffs)
                    {
                        report.AppendLine("----------------------");
                        report.AppendLine("Selector: " + diff.selector);
                        report.AppendLine("Html 1: " + maxLength(diff.compareSourceHtml, 60));
                        report.AppendLine("Html 2: " + maxLength(diff.compareWithHtml, 60));
                    }
                }
                report.AppendLine("=======================");
            }

            report.AppendLine("Total number of diffs: " + totalDiffs.ToString());
            report.AppendLine("=======================");

            return report.ToString();
        }
        private string maxLength(string text, int maxLength)
        {
            if (text.Length > maxLength) return text.Substring(0, maxLength) + "...";
            return text;
        }

        

        public SiteCompare(string comparePath1, string comparePath2, string[] urlList, string selector, string[] excludeSelectors = null)
        {
            // TODO: Complete member initialization
            this.comparePath1 = comparePath1;
            this.comparePath2 = comparePath2;
            this.urlList = urlList;
            this.selector = selector;
            this.excludeSelectors = excludeSelectors;
            this.CompareResult = new List<SiteCompareResult>();

            foreach (var url in urlList)
            {

                var path1 = Utilities.GetFullPath(comparePath1, url);
                var path2 = Utilities.GetFullPath(comparePath2, url);

                var html1 = Utilities.GetHtmlFromFileOrUrl(path1);
                var html2 = Utilities.GetHtmlFromFileOrUrl(path2);

                var htmlDiff = new HtmlComparer(html1, html2, selector, excludeSelectors);

                this.CompareResult.Add(new SiteCompareResult
                {
                    path1 = path1,
                    path2 = path2,
                    Diffs = htmlDiff.Diffs

                });

            }

        }

        public SiteCompare(string url1, string url2, string selector)
        {
            // TODO: Complete member initialization
            this.comparePath1 = url1;
            this.comparePath2 = url2;
            this.selector = selector;
            this.CompareResult = new List<SiteCompareResult>();

            using (var client = new WebClient())
            {

                var html1 = client.DownloadString(url1);
                var html2 = client.DownloadString(url2);

                var htmlDiff = new HtmlComparer(html1, html2, selector);

                this.CompareResult.Add(new SiteCompareResult
                {
                    path1 = url1,
                    path2 = url2,
                    Diffs = htmlDiff.Diffs

                });

            }


        }        
    }
}
