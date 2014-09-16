using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DryHtml.SiteDiff.Tests
{
    [TestClass]
    public class BasicCompareTests
    {
        [TestMethod]
        public void CreateSnapshot()
        {
            var rootUrl = @"http://en.wikipedia.org/wiki/";
            var urlList = new[] { "Sweden", "Norway", "Finland" };

            var dumpToPath = @"c:\data\filedump\wikipedia\1\";

            var selector = "*";

            var siteSnapshot = new SiteSnapshot(rootUrl, urlList, selector, dumpToPath);

            Assert.IsNotNull(siteSnapshot);

        }

        [TestMethod]
        public void CreateSnapshots()
        {
            var rootUrl = @"http://en.wikipedia.org/wiki/";
            var urlList = new[] { "Sweden", "Norway", "Finland" };

            var dumpToPath1 = @"c:\data\filedump\wikipedia\1\";
            var dumpToPath2 = @"c:\data\filedump\wikipedia\2\";

            var selector = "*";

            var siteSnapshot1 = new SiteSnapshot(rootUrl, urlList, selector, dumpToPath1);
            var siteSnapshot2 = new SiteSnapshot(rootUrl, urlList, selector, dumpToPath2);

            Assert.IsNotNull(siteSnapshot1);
            Assert.IsNotNull(siteSnapshot2);

        }

        [TestMethod]
        public void CompareSnapshots()
        {

            var comparePath1 = @"c:\data\filedump\wikipedia\1\";
            var comparePath2 = @"c:\data\filedump\wikipedia\2\";
            var urlList = new[] { "Sweden.html", "Norway.html", "Finland.html" };

            var selector = "*";

            var siteDiff = new SiteCompare(comparePath1, comparePath2, urlList, selector);

            Assert.IsNotNull(siteDiff);

            var report = siteDiff.TextReport;

            Assert.IsNotNull(report);


            // sample result:

            //=======================
            //Compare result
            //=======================
            //Path 1: c:\data\filedump\wikipedia\1\Sweden.html
            //Path 2: c:\data\filedump\wikipedia\2\Sweden.html
            //Equals
            //=======================
            //Path 1: c:\data\filedump\wikipedia\1\Norway.html
            //Path 2: c:\data\filedump\wikipedia\2\Norway.html
            //Equals
            //=======================
            //Path 1: c:\data\filedump\wikipedia\1\Finland.html
            //Path 2: c:\data\filedump\wikipedia\2\Finland.html
            //Number of diffs: 2
            //----------------------
            //Selector: html.client-nojs > head > title
            //Html 1: Finland - Wikip...
            //Html 2: Funland - Wikip...
            //----------------------
            //Selector: H1#firstHeading span
            //Html 1: Finland
            //Html 2: Funland
            //=======================
            //Total number of diffs: 2
            //=======================


        }

    }
}
