using DryHtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsQuery.ExtensionMethods;

namespace DryHtml.Views.Partials
{
    public class H2 : DryHtml
    {
        public H2(string header) : base("<h2>{header}</h2>", new { header }){}
    }
}