using CsQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryHtml.ViewGen
{
    public class ViewGenerator
    {
        CQ prototypeHtml;
        CQ csHtmlView;
        string selector;
        Dictionary<string, string> modelDescriber;
        public ViewGenerator(string prototypeHtml, Dictionary<string, string> modelDescriber)
        {
            this.prototypeHtml = prototypeHtml;
            this.modelDescriber = modelDescriber;
        }
        public ViewGenerator(string prototypeHtml, string selector, Dictionary<string, string> modelDescriber)
        {
            this.prototypeHtml = prototypeHtml;
            this.modelDescriber = modelDescriber;
            this.selector = selector;
        }
        private void generateCshtml()
        {
            this.csHtmlView = prototypeHtml;
            if (selector != "") this.csHtmlView = prototypeHtml.Select(selector);
            foreach (var property in modelDescriber)
            {
                this.csHtmlView[property.Value].Text("@Model." + property.Key);
            }

        }
        public string CsHtmlView
        {
            get
            {
                if (csHtmlView == null) generateCshtml();
                return csHtmlView.RenderSelection();
            }
        }
    }
}
