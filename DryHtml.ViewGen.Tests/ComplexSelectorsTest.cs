using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DryHtml.ViewExtractor.Tests
{
    [TestClass]
    public class ComplexSelectorsTest
    {
        [TestMethod]
        [DeploymentItem("SamplePrototype.html")]
        public void TestMethod1()
        {
            // Arrange

            var prototypeHtml = System.IO.File.ReadAllText("SamplePrototype.html");

            var vg = new ViewGen.ViewGenerator(prototypeHtml, new PrototypeExtractor("People", "#people", "", p =>
            {
                p.AddChildAt("PersonList", "ul", "li", "List<PersonList>", ul =>
                {
                    ul.AddChildNext("Name", "span"); // PersonList_Name(string name)
                    ul.AddChildNext("Address", "span"); // PersonList_Address(string name)
                    ul.AddChildNext("Description", "#text"); // 
                    ul.AddChildNext("Email", "a");
                });
            }));

            var model = vg.CsModel;
            var view = vg.CsHtmlView;

        }
    }
}
