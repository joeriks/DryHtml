using DryHtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsQuery.ExtensionMethods;

namespace DryHtml.Views.Partials
{
    public class H2 : DryHtmlDocument
    {
        public H2(string header)
        {
            this.DOM = String.Format("<h2>{0}</h2>", header);
        }
    }
}