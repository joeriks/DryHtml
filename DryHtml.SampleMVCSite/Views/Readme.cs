using DryHtml;
using HtmlAgilityPack;
using DryHtml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DryHtml.Views
{
    public class Readme : DryHtmlResult
    {
        public Readme(ReadmeViewModel model)
            : base("~/Views/Templates/readme.html")
        {

            this.SetHtml("//title", model.Title);
            this.SetHtml("{header}/h1", model.Header);
            this.SetHtml("{partial}", new Partials.H2(model.PartialHeader).Html);


        }



    }
}