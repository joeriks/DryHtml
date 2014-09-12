using DryHtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DryHtml.Views.Partials
{
    public class H2 : DryHtmlDocument
    {
        public H2(string header)
        {
            this.Html = "<h2>" + header + "</h2>";
        }
    }
}