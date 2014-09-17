using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DryHtml.Tests
{
    [TestClass]
    public class ComplexJsonViewGenTests
    {

        [TestMethod]
        [DeploymentItem("ComplexViewGen.json")]
        [DeploymentItem("SamplePrototype.html")]        
        public void TestGenerationFromViewModelWithPrototype()
        {

            // Arrange                                   
            var generation = new ViewGen.ViewGenerator("ComplexViewGen.json");
            generation.OutputRootPath = @"c:\data\";

            // Act            
            generation.GenerateAll();
            
            // Assert
            // Manually check c:\data\ :-p

        }


    }
}
