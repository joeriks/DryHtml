using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DryHtml.Tests
{
    [TestClass]
    public class DictionaryViewTests
    {


        [TestMethod]
        [DeploymentItem("SamplePrototype.html")]
        [DeploymentItem("SampleGeneratedView.cshtml")]
        public void TestGenerationFromDictionary()
        {


            // Arrange

            var prototypeHtml = System.IO.File.ReadAllText("SamplePrototype.html");
            var expectedResult = System.IO.File.ReadAllText("SampleGeneratedView.cshtml");

            var modelDictionary = new Dictionary<string, string>();
            modelDictionary.Add("Header", "h1");
            modelDictionary.Add("Description", "p");

            // Act

            var generatedResult = new ViewGen.ViewGenerator(prototypeHtml, "#header", modelDictionary).CsHtmlView;

            // Assert

            Assert.AreEqual(expectedResult, generatedResult);


        }


    }
}
