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
        public static IHtmlString h1(this HtmlHelper self, string header)
        {
            return new HtmlString("<h1>{header}</h1>".Replace(new { header }));
        }
        public static IHtmlString h2(this HtmlHelper self, string header)
        {
            return new HtmlString("<h2>{header}</h2>".Replace(new { header }));
        }
        public static IHtmlString h3(this HtmlHelper self, string header)
        {
            return new HtmlString("<h3>{header}</h3>".Replace(new { header }));
        }
        public static IHtmlString h4(this HtmlHelper self, string header)
        {
            return new HtmlString("<h4>{header}</h4>".Replace(new { header }));
        }

    }
}