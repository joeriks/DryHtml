using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DryHtml.Tests
{
    [TestClass]
    public class AutoViewTests
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



        [TestMethod]
        [DeploymentItem("SamplePrototype.html")]
        [DeploymentItem("SampleGeneratedStronglyTypedView2.cshtml")]
        public void TestGenerationFromViewModelWithPrototype()
        {

            // Arrange            
            var expectedResult = System.IO.File.ReadAllText("SampleGeneratedStronglyTypedView2.cshtml");

            // Act
            var generatedResult = new ViewGen.ViewGenerator<ReadMeHeader2>().CsHtmlView;

            // Assert
            Assert.AreEqual(expectedResult, generatedResult);


        }

        [ViewModelGenerator("SamplePrototype.html", "#header")]
        public class ReadMeHeader2
        {
            [ViewModelProperty("h1")]
            public string Header { get; set; }
            [ViewModelProperty("p")]
            public string Description { get; set; }
        }



        [TestMethod]
        [DeploymentItem("SamplePrototype.html")]
        [DeploymentItem("SampleGeneratedStronglyTypedView.cshtml")]
        public void TestGenerationFromViewModel()
        {

            // Arrange

            var prototypeHtml = System.IO.File.ReadAllText("SamplePrototype.html");
            var expectedResult = System.IO.File.ReadAllText("SampleGeneratedStronglyTypedView.cshtml");

            // Act

            var generatedResult = new ViewGen.ViewGenerator<ReadMeHeader>(prototypeHtml).CsHtmlView;

            // Assert

            Assert.AreEqual(expectedResult, generatedResult);


        }


        [ViewModelGenerator("#header")]
        public class ReadMeHeader
        {
            [ViewModelProperty("h1")]
            public string Header { get; set; }
            [ViewModelProperty("p")]
            public string Description { get; set; }
        }

        



    }
}
