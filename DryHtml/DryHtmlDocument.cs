using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DryHtml
{
    
    public class DryHtml
    {        

        private CsQuery.CQ _dom;

        public CsQuery.CQ DOM
        {
            get
            {
                return _dom;
            }
            set
            {
                _dom = value;
            }
        }
        public DryHtml(string template, object model)
        {
            _dom = new CsQuery.CQ();
            _dom = template.Replace(model);
        }
        public DryHtml(string templateFilePath = "")
        {

            //_template = new HtmlDocument();

            _dom = new CsQuery.CQ();

            if (templateFilePath.Length > 255 || templateFilePath.Contains('<'))
            {
                _dom = templateFilePath;

            }
            else if (templateFilePath != "")
            {

                var file = System.IO.File.ReadAllText(templateFilePath);
                _dom = file;

            }
        }

        
        public string Html
        {
            get
            {
                return _dom.Render();
            }
            set
            {
                _dom = value;
            }
        }



    }
}
