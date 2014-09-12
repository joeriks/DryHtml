using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Diagnostics;

namespace DryHtml.Tests
{
    [TestClass]
    public class BasicTest
    {
        [TestMethod]
        public void TestMethod1()
        {            

            // create the html document

            var sp = new Stopwatch();
            sp.Start();

            var model = new DocumentViewModel { Title = "Readme Title", Header = "This is a header", PartialHeader = "Header from partial" };
            var readme = new ReadmeView(model);

            Console.WriteLine(sp.ElapsedMilliseconds);

            sp.Restart();
            var model2 = new DocumentViewModel { Title = "Readme Title", Header = "This is a header", PartialHeader = "Header from partial" };
            var readme2 = new ReadmeView(model2);
            Console.WriteLine(sp.ElapsedMilliseconds);

            // get it back

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(readme.Html);

            // assert

            Assert.AreEqual(model.Header, doc.DocumentNode.SelectSingleNode("//*[@id='header']/h1").InnerText);
            Assert.AreEqual(model.Title, doc.DocumentNode.SelectSingleNode("//title").InnerText);
            Assert.AreEqual(model.PartialHeader, doc.DocumentNode.SelectSingleNode("//*[@id='partial']/h2").InnerText);

            // for debug - print full html doc to temp file

            //var tempFilename = System.IO.Path.GetTempFileName();
            //System.IO.File.WriteAllText(tempFilename, readme.Html);
            //Console.Write(tempFilename);

        }
    }
}
