using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DryHtml.Tests
{
    [TestClass]
    public class DiffTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var compareSource = "<div id=\"header\"><h1>Header</h1></div>";
            var compareWith = "<div id=\"header\"></div>";

            var htmlComparer = new HtmlComparer(compareSource, compareWith);

            var result = htmlComparer.Result();
            Assert.AreEqual("<h1>Header</h1>", result);
        }
    }
}
