using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryHtml.Tests
{
    public class H2 : DryHtmlDocument
    {
        public H2(string header)
        {
            this.Html = "<h2>" + header + "</h2>";
        }
    }
}
