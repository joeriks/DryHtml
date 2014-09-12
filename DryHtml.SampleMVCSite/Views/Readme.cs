using DryHtml;
using HtmlAgilityPack;
using DryHtml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DryHtml.Views.Partials;

namespace DryHtml.Views
{
    public class Readme : DryHtmlResult
    {
        public Readme(ReadmeViewModel model)
            : base("~/Views/Templates/readme.html")
        {


            this.DOM["title"].Text(model.Title);
            this.DOM["#header > h1"].Text(model.Header);
            this.DOM["#partial"].Html(new H2(model.PartialHeader).Html);


        }



    }
}