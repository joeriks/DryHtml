using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DryHtml.Tests
{
    [TestClass]
    public class AutoViewTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var readMeHeader = new ReadMeHeader{Header="Header",Description="Description"};

            var hagp = new HtmlAgilityPack.HtmlDocument();
            hagp.LoadHtml(readMeHeader.Render().ToString());
            
            Assert.AreEqual("Header", hagp.DocumentNode.SelectSingleNode("//*[@id='header']/h1").InnerText);
            Assert.AreEqual("Description", hagp.DocumentNode.SelectSingleNode("//*[@id='header']/p").InnerText);

        }

        /// <summary>
        /// <body>
        ///  Takes an HTML string and genereates view from it based on viewmodel attributes
        ///  <div id="header">
        ///    <h1>Your ASP.NET application</h1>
        ///    <p>Congratulations! You've created a project</p>
        ///  </div>    

        /// </summary>

        [ViewModelGenerator("DryHtml.Tests.Readme.html", "#header")]
        public class ReadMeHeader : ViewGenerator
        {
            [ViewModelGenerator("h1")]
            public string Header { get; set; }
            [ViewModelGenerator("p")]
            public string Description { get; set; }
        }

    }
}
