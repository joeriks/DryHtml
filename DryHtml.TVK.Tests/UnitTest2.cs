using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DryHtml.TVK.Tests
{
    [TestClass]
    public class FormatTests
    {
        [TestMethod]
        public void TestListOfString()
        {
            var p = new List<string>() { "Foo", "Bar" };
            var result = p.Format("<li>{0}</li>");

            var expectedResult = "<li>Foo</li><li>Bar</li>";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestListOfDecimal()
        {
            var p = new List<decimal> { (decimal)1.251, (decimal)1.501 };

            var result = p.Format("<li>{0:0.00}</li>");

            var expectedResult = "<li>1,25</li><li>1,50</li>";
            Assert.AreEqual(expectedResult, result);
        }


        [TestMethod]
        public void TestDictionary()
        {
            var p = new Dictionary<string, string>() { { "Foo", "Bar" }, { "Bar", "Baz" } };
            var result = p.Format("<label>{Key}</label>{Value}");

            var expectedResult = "<label>Foo</label>Bar<label>Bar</label>Baz";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestAnonymousObject()
        {
            var p = new { Foo = "Bar", Baz = "Bah" };
            var result = p.Format("{Foo},{Baz}");

            var expectedResult = "Bar,Bah";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestAnonymousObjectWithList()
        {
            var p = new { Foo = "Bar", Baz = new List<string>{"Foo","Bar"} };
            var result = p.Format("{Foo}<ul>{Baz:<li>{0}</li>}</ul>");

            var expectedResult = "Bar<ul><li>Foo</li><li>Bar</li></ul>";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestAnonymousObjectWithListVer2()
        {
            var p = new { Foo = "Bar", Baz = new List<string> { "Foo", "Bar" } };

            var templateP = new { p.Foo, Baz = p.Baz.Format("<li>{0}</li>") };
            var result = templateP.Format("{Foo}<ul>{Baz}</ul>");

            var expectedResult = "Bar<ul><li>Foo</li><li>Bar</li></ul>";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestAnonymousObjectWithListVer3()
        {
            var p = new { Foo = "Bar", Baz = new List<string> { "Foo", "Bar" } };
            var result = p.Format(p1 => new { p1.Foo, Baz = p1.Baz.Format("<li>{0}</li>") }, 
                "{Foo}<ul>{Baz}</ul>");

            var expectedResult = "Bar<ul><li>Foo</li><li>Bar</li></ul>";
            Assert.AreEqual(expectedResult, result);
        }


        [TestMethod]
        public void TestAnonymousComplexObject()
        {
            var p = new { Foo = "Bar", Baz = new {Level=2} };
            var result = p.Format("{Foo}{Baz:<div>{Level}</div>}");

            var expectedResult = "Bar<div>2</div>";
            Assert.AreEqual(expectedResult, result);
        }

        public class Complex
        {
            public string Foo { get; set; }
            public BazType Baz { get; set; }
            public List<string> Lis { get; set; }
            public class BazType
            {
                public int Level { get; set; }
            }
        }

        [TestMethod]
        public void TestComplexObject()
        {
            var p = new Complex { Foo = "Bar", Baz = new Complex.BazType { Level = 2 } };
            var result = p.Format("{Foo}{Baz:<div>{Level}</div>}");

            var expectedResult = "Bar<div>2</div>";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestComplexObjectVer2()
        {
            var p = new Complex { Foo = "Bar", Baz = new Complex.BazType { Level = 2 } };
            var result = p.Format("{Foo}<div>{Baz.Level}</div>");

            var expectedResult = "Bar<div>2</div>";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestComplexObjectNull()
        {
            var p = new Complex { Foo = "Bar", Baz = null};
            var result = p.Format("{Foo}{Baz:<div>{Level}</div>}");

            var expectedResult = "Bar";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestComplexObjectNullVer2()
        {
            var p = new Complex { Foo = "Bar", Baz = null, Lis = new List<string>{"aa","bb"} };
            var result = p.Format("{Foo}{Baz:<div>{Level}</div>}{Lis:{0}}");

            var expectedResult = "Baraabb";
            Assert.AreEqual(expectedResult, result);
        }
        [TestMethod]
        public void TestComplexObjectNullVer3()
        {
            var p = new Complex { Foo = "Bar", Baz = null, Lis = new List<string>() };
            var result = p.Format("{Foo}{Baz:<div>{Level}</div>}{Lis:{0}}");

            var expectedResult = "Bar";
            Assert.AreEqual(expectedResult, result);
        }


        public class Complex2
        {
            public string Foo { get; set; }
            public BazType Baz { get; set; }

            public class BazType
            {
                public int Level { get; set; }
                public BazType SubBaz { get; set; }
            }
        }
        
        [TestMethod]
        public void TestComplex2Object()
        {
            var p = new Complex2 { Foo = "Bar", Baz = new Complex2.BazType { Level = 2, SubBaz = new Complex2.BazType { Level = 3 } } };
            var result = p.Format("{Foo}{Baz:<div>{Level}</div>{SubBaz:{Level}}}");

            var expectedResult = "Bar<div>2</div>3";
            Assert.AreEqual(expectedResult, result);
        }


        public class Node
        {
            public string Name { get; set; }
            public List<Node> Children { get; set; }
        }

        [TestMethod]
        public void TestRecursive()
        {
            var p = new Node { Name = "Top", Children = new List<Node> { 
                new Node { Name = "Child" }, 
                new Node { Name = "Second child" } } };
            var result = p.Format("[{Name} Children:{Children}]");

            var expectedResult = "[Top Children:[Child Children:][Second child Children:]]";
            Assert.AreEqual(expectedResult, result);
        }
    }
}
