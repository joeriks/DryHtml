using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DryHtml;
using System.Web.Mvc;

namespace DryHtml.Views.Helpers
{
    public static partial class RazorHtmlExtensions
    {
        public static IHtmlString doubleHeader(this HtmlHelper self, string header, string secondHeader)
        {
            //return self.h1(header).Concat(self.h2(secondHeader));

            return new HtmlString("<h1>{header}</h1><h2>{secondHeader}</h2>".Replace(new { header, secondHeader }));
        }

    }
}