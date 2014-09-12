using DryHtml;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DryHtml.Tests
{
    public class Readme : DryHtmlDocument
    {
        private static string GetResource(string resourceNameSpace)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceNameSpace))
                using (StreamReader reader = new StreamReader(stream))
                    return reader.ReadToEnd();
        }
        public Readme(DocumentViewModel model)
            : base(GetResource("DryHtml.Tests.Readme.html"))
        {
            this.SetHtml("//title", model.Title);
            this.SetHtml("{header}/h1", model.Header);
            this.SetHtml("{partial}", new H2(model.PartialHeader).Html);
        }



    }
}