using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DryHtml.HtmlDiff.Tests
{
    [TestClass]
    public class BasicDiffs
    {
        [TestMethod]
        public void FindDiff()
        {
            var x = new HtmlDiff.Comparer("<html><body><p>A</p></html>", "<html><p>B</p></body></html>");
            Assert.AreEqual("A", x.Diffs[0].compareSourceHtml);
            Assert.AreEqual(true, x.Diffs[0].isText);
            Assert.AreEqual("html > body > p", x.Diffs[0].selector);
            Assert.AreEqual(1, x.Diffs.Count);
        }
        [TestMethod]
        public void FindNthChild()
        {
            var x = new HtmlDiff.Comparer("<html><body><p>A</p><p>A</p></html>", "<html><p>A</p><p>B</p></body></html>");
            Assert.AreEqual("A", x.Diffs[0].compareSourceHtml);
            Assert.AreEqual(true, x.Diffs[0].isText);
            Assert.AreEqual("html > body > p:nth-child(1)", x.Diffs[0].selector);
            Assert.AreEqual(1, x.Diffs.Count);
        }
    }
}
