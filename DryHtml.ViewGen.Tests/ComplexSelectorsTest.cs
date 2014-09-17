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
            //var expectedResult = System.IO.File.ReadAllText("SampleGeneratedView.cshtml");

            var viewStart = new ViewGen.ViewGenerator(prototypeHtml, "#people", gen =>
            {
                gen.
            });
            
            // Act

            var generatedResult = new ViewGen.ViewGenerator(prototypeHtml, "#header", modelDictionary).CsHtmlView;

            // Assert

            Assert.AreEqual(expectedResult, generatedResult);


        }
    }
}
