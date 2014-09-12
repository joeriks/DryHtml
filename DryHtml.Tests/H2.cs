using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryHtml.Tests
{
    public class H2 : DryHtml
    {
        public H2(string header) : base("<h2>{header}</h2>", new { header }) { }
    }
}
