using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DryHtml.SiteDiff.Tests
{
    [TestClass]
    public class SiteDiffTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            
            var sitemap = @"<p><a href='baz'>baz</a></p>
<ul>
<li><a href='foo'>foo</a></li>
<li><a href='bar'>bar</a>
  <ul>
    <li><a href='bar1'>bar1</a></li>
    <li><a href='bar2'>bar2</a></li>
  </ul></li>
</ul>";

            var urls = SiteDiff.SiteSnapshot.UrlListFromSiteMap(sitemap, "ul:first");


            Assert.AreEqual(4, urls.Length);
            Assert.AreEqual("foo", urls[0]);
            Assert.AreEqual("bar2", urls[3]);

        }
        [TestMethod]
        public void TestMethod2()
        {

            var sitemap = @"<p><a href='baz'>baz</a></p>
<ul>
<li><a href='http://site/foo'>foo</a></li>
<li><a href='http://site/bar'>bar</a>
  <ul>
    <li><a href='http://site/bar/bar1'>bar1</a></li>
    <li><a href='http://site/bar/bar2'>bar2</a></li>
  </ul></li>
</ul>";

            var urls = SiteDiff.SiteSnapshot.UrlListFromSiteMap(sitemap, "ul:first", "http://site");


            Assert.AreEqual(4, urls.Length);
            Assert.AreEqual("/foo", urls[0]);
            Assert.AreEqual("/bar/bar2", urls[3]);

        }

        [TestMethod]
        public void SiteMapUrls()
        {
            var s = new SiteDiff.SiteSnapshot("https://www.golvbranschen.se/", "", "https://www.golvbranschen.se/hem/webbplatskarta",".sitemap", @"c:\admin\www.golvbranschen.se");
        }
    }
}
