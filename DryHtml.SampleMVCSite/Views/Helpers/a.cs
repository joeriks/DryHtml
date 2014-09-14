using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DryHtml.Views.Helpers
{
    public static partial class RazorHtmlExtensions
    {
        public static IHtmlString a(this HtmlHelper self, string url, string title)
        {            
            return new HtmlString("<a href=\"{url}\">{title}</a>"
                .Replace(new { url, title}));
        }
    }
}