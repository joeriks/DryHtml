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
        
        public Readme(DocumentViewModel model)
            : base(Helpers.GetResource("DryHtml.Tests.Readme.html"))
        {
            this.SetHtml("//title", model.Title);
            this.SetHtml("{header}/h1", model.Header);
            this.SetHtml("{partial}", new H2(model.PartialHeader).Html);
        }



    }
}