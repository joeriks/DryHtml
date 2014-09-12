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
    public class ReadmeView : DryHtml
    {

        public ReadmeView(DocumentViewModel model)
            : base(Helpers.GetResource("DryHtml.Tests.Readme.html"))
        {

            this.DOM["title"].Text(model.Title);
            this.DOM["#header > h1"].Text(model.Header);
            this.DOM["#partial"].Html(new H2(model.PartialHeader).Html);
        }



    }
}