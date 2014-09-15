using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DryHtml.Tests
{
    [TestClass]
    public class JsonViewGenTests
    {

        [TestMethod]
        [DeploymentItem("ViewGen.json")]
        [DeploymentItem("SamplePrototype.html")]        
        public void TestGenerationFromViewModelWithPrototype()
        {

            // Arrange                                   
            var generation = new ViewGen.ViewGenerator("ViewGen.json");
            generation.OutputRootPath = @"c:\data\";

            // Act            
            generation.GenerateAll();
            
            // Assert
            // Manually check c:\data\ :-p

            //var generatedView = generation.CsHtmlView;
            //var generatedViewModel = generation.CsViewModel;

            // Assert
            //Assert.AreEqual(expectedResult, expectedViewResult);


        }


    }
}
