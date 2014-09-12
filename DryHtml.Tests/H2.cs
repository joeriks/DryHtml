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
            
            this.DOM = String.Format("<h2>{0}</h2>", header);

        }
    }
}
