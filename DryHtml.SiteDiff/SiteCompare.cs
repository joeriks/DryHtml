using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DryHtml.SiteDiff.Tests
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
            public List<HtmlDiff.Comparer.Diff> Diffs { get; set; }
        }

        private string textReport;

        public string TextReport
        {
            get
            {
                if (string.IsNullOrEmpty(textReport)) textReport = generateTextReport();
                return textReport;
            }
        }
        private string generateTextReport(){

            var report = new StringBuilder();
            var totalDiffs = 0;
            report.AppendLine("=======================");
            report.AppendLine("Compare result");
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
                        report.AppendLine("Html 1: " + maxLength(diff.compareSourceHtml,15));
                        report.AppendLine("Html 2: " + maxLength(diff.compareWithHtml, 15));
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
        public SiteCompare(string comparePath1, string comparePath2, string[] urlList, string selector)
        {
            // TODO: Complete member initialization
            this.comparePath1 = comparePath1;
            this.comparePath2 = comparePath2;
            this.urlList = urlList;
            this.selector = selector;
            this.CompareResult = new List<SiteCompareResult>();

            foreach (var url in urlList)
            {

                var path1 = Path.Combine(comparePath1, url);
                var path2 = Path.Combine(comparePath2, url);

                var html1 = System.IO.File.ReadAllText(path1);
                var html2 = System.IO.File.ReadAllText(path2);

                var htmlDiff = new HtmlDiff.Comparer(html1, html2, selector);

                this.CompareResult.Add(new SiteCompareResult
                {
                    path1 = path1,
                    path2 = path2,
                    Diffs = htmlDiff.Diffs
                    
                });

            }

        }

        public List<HtmlDiff.Comparer.Diff> Diffs { get; set; }
    }
}
