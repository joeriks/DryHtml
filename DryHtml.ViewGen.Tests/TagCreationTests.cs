using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryHtml.ViewExtractor.Tests
{
    [TestClass]
    public class TagCreationTests
    {

        public class El
        {
            public string TagName { get; set; }
            public Dictionary<string, string> Attributes { get; set; }
            public List<El> Tags { get; set; }
            public El(string tagName, Dictionary<string, string> attributes, Action<El> self = null)
            {
                TagName = tagName;
                Attributes = new Dictionary<string, string>();
                Tags = new List<El>();
            }
            public El(string tagName, Action<El> self = null)
            {
                Tags = new List<El>();
            }
            public El()
            {
                Tags = new List<El>();
            }
            public El(string tagName, string text = "", string id = "", string cssClass = "", string href = "", string attr = "", Action<El> self = null)
            {
                Tags = new List<El>();
            }
            public void Add(string tagName, Dictionary<string, string> attributes, Action<El> self = null)
            {
            }
            public void Add(string tagName, string text = "", string id = "", string cssClass = "", string href = "", string attr = "", Action<El> self = null)
            {
            }

            public void A(string text = "", string id = "", string cssClass = "", string href = "", string attr = "", Action<El> self = null)
            {
                Add("a", text, id, cssClass, href, attr, self);
            }
            public void Span(string text = "", string id = "", string cssClass = "", string href = "", string attr = "", Action<El> self = null)
            {
                Add("span", text, id, cssClass, href, attr, self);
            }
            public void Div(string text = "", string id = "", string cssClass = "", string href = "", string attr = "", Action<El> self = null)
            {
                Add("div", text, id, cssClass, href, attr, self);
            }
            public void P(string text = "", string id = "", string cssClass = "", string href = "", string attr = "", Action<El> self = null)
            {
                Add("p", text, id, cssClass, href, attr, self);
            }

            public void P(Action<El> self)
            {
                Add("p", self: self);
            }
            public string InnerText { get; set; }
            public void Text(string text)
            {
                this.InnerText = text;
            }
            public El Attr(string name, string value)
            {
                Attributes.Add(name, value);
                return this;
            }
            public El Class(string value)
            {
                Attributes.Add("class", value);
                return this;
            }
            public El Id(string value)
            {
                Attributes.Add("id", value);
                return this;
            }
            public static El Create(Action<El> self)
            {
                var s = new El();
                self(s);
                return s;
            }
        }

        [TestMethod]
        public void TestMe()
        {
            //var bodyTag = new El("body");

            //var bodyTagWithChildren = new El("body", t =>
            //{
            //    t.Add("p", "Foo");
            //    t.Add("span", "Bar");
            //});

            //var tagWithCustomAttribute = new El("p", attr: "data-custom='foo'");
            //var tagWithClass = new El("p", cssClass: "red");
            //var tagWithId = new El("p", id: "foo");

            //var tagWithIdAndChildren = new El("p", id: "foo", self: s =>
            //{
            //    s.Add("p", "Foo");
            //    s.Add("span", "Bar");
            //});

            //var ppp = El.Create(html =>
            //{
            //    html.P(add =>
            //    {
            //        add.Span("foo");
            //        add.Text("foo");
            //        add.P(add =>
            //        {
            //            add.Span("inner");
            //        });
            //    });
            //});



        }
    }
}
