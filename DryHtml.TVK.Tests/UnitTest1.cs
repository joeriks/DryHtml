using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DryHtml.TVK.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestPersonTVM()
        {
            var p = new PersonTVM { Name = "Foo", Address = "Bar", City = "Baz" };
            var result = p.ToString();
            var expectedResult = "<div>Foo, Bar, Baz</div>";
            Assert.AreEqual(expectedResult, result);
        }

        public class PersonTVM : DryHtml.TemplatedViewModel
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string City { get; set; }

            override public string Template()
            {
                return "<div>{Name}, {Address}, {City}</div>";
            }

        }


        [TestMethod]
        public void TestPersonWithFunctionTVM()
        {
            var p = new PersonWithFunctionTVM { Name = "Foo", Address = "Bar", City = "Baz" };
            var result = p.ToString();
            var expectedResult = "<div>Foo, Bar, Baz, baz</div>";
            Assert.AreEqual(expectedResult, result);
        }

        public class PersonWithFunctionTVM : DryHtml.TemplatedViewModel
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string LowerCaseCity { get { return City.ToLower(); } }

            override public string Template()
            {
                return "<div>{Name}, {Address}, {City}, {LowerCaseCity}</div>";
            }

        }
        [TestMethod]
        public void TestReplace()
        {
            var p = "<li>{foo}</li>".TemplateReplace(new { foo = "123 {x}".TemplateReplace(new { x = "kr" }) });
        }

        [TestMethod]
        public void TestWithList()
        {
            var p = "<li>{foo}</li>";
            var foo = new List<string> { "foo", "bar" };
            var x = TemplateReplaceExtension.TemplateReplace(p, new { foo });
            Assert.AreEqual("<li>foobar</li>", x);
        }

        [TestMethod]
        public void TestPersonWithChildTVM()
        {
            var p = new PersonWithChildTVM { Name = "Foo", Address = "Bar", City = "Baz", Child = new PersonWithChildTVM { Name = "ChildFoo", Address = "ChildBar", City = "ChildBaz" } };
            var result = p.ToString();
            var expectedResult = "<div>Foo, Bar, Baz<div>ChildFoo, ChildBar, ChildBaz</div></div>";
            Assert.AreEqual(expectedResult, result);
        }

        public class PersonWithChildTVM : DryHtml.TemplatedViewModel
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public PersonWithChildTVM Child { get; set; }

            override public string Template()
            {
                return "<div>{Name}, {Address}, {City}{Child}</div>";
            }

        }
        [TestMethod]
        public void TestListTVM()
        {
            var p = new ListTVM { Names = new List<ListTVM.Name> { new ListTVM.Name { Value = "Foo" }, new ListTVM.Name { Value = "Bar" } } };
            var result = p.ToString();
            var expectedResult = "<ul><li>Foo</li><li>Bar</li></ul>";
            Assert.AreEqual(expectedResult, result);


        }

        public class ListTVM : DryHtml.TemplatedViewModel
        {
            public class Name : DryHtml.TemplatedViewModel
            {
                public string Value { get; set; }
                public override string Template()
                {
                    return "<li>{Value}</li>";
                }
            }

            public List<Name> Names { get; set; }

            override public string Template()
            {
                return "<ul>{Names}</ul>";
            }

        }


    }
}
