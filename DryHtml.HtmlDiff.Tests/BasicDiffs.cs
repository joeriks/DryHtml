using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsQuery;

namespace DryHtml.HtmlDiff.Tests
{
    [TestClass]
    public class BasicDiffs
    {
        [TestMethod]
        public void FindDiff()
        {
            var compareHtml = "<html><body><p>A</p></html>";
            var x = new HtmlDiff.Comparer(compareHtml, "<html><p>B</p></body></html>");
            Assert.AreEqual("A", x.Diffs[0].compareSourceHtml);
            Assert.AreEqual(true, x.Diffs[0].isText);
            Assert.AreEqual("html > body > p", x.Diffs[0].selector);

            Assert.AreEqual(1, x.Diffs.Count);

            var useComputedSelector = ((CQ)compareHtml)[x.Diffs[0].selector].Text();
            Assert.AreEqual("A", useComputedSelector);

            var useWrittenSelector = ((CQ)compareHtml)["html > body > p"].Text();
            Assert.AreEqual("A", useWrittenSelector);


        }
        [TestMethod]
        public void FindNthChild()
        {
            var compareHtml = "<html><body><p>A</p><p>C</p></html>";
            var x = new HtmlDiff.Comparer(compareHtml, "<html><p>A</p><p>B</p></body></html>");
            Assert.AreEqual("C", x.Diffs[0].compareSourceHtml);
            Assert.AreEqual(true, x.Diffs[0].isText);
            Assert.AreEqual("html > body > p:nth-child(2)", x.Diffs[0].selector);
            Assert.AreEqual(1, x.Diffs.Count);

            var useComputedSelector = ((CQ)compareHtml)["html > body > p:nth-child(2)"].Text();
            Assert.AreEqual("C", useComputedSelector);


        }

        [TestMethod]
        public void ExcludeSelectors()
        {
            var html1 = "<html><body><p>A</p><p class='exclude'>C</p></html>";
            var html2 = "<html><p>A</p><p class='exclude'>D</p></body></html>";
            var x = new HtmlDiff.Comparer(html1, html2);
            Assert.AreEqual("C", x.Diffs[0].compareSourceHtml);
            Assert.AreEqual(true, x.Diffs[0].isText);
            Assert.AreEqual("html > body > p.exclude", x.Diffs[0].selector);

            Assert.AreEqual(1, x.Diffs.Count);

            var excludeSelectors = new[] { ".exclude" };

            var ex = new HtmlDiff.Comparer(html1, html2, "",excludeSelectors);

            Assert.AreEqual(0, ex.Diffs.Count);

        }

    }
}
