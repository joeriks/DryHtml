using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DryHtml
{
    public static class DryHtmlExtensions
    {
        // @header/h1 --> //[@id='header']/h1
        public static HtmlNode SelectSingleNode(this DryHtml document, string xpath)
        {
            //if (dataDryHtml != "")
            //{
            //    xpath = "//*[@data-dryhtml='" + dataDryHtml + "']" + xpath;
            //}

            return document.HtmlDocument.DocumentNode.SelectSingleNode(xpath);
        }

        public static string GetInnerText(this DryHtml document, string xpath)
        {
            xpath = xPathId(xpath);
            return SelectSingleNode(document, xpath).InnerText;
        }
        public static void SetHtml(this DryHtml document, string xpath, string value)
        {
            xpath = xPathId(xpath);
            SelectSingleNode(document, xpath).InnerHtml = value;
        }

        private static string xPathId(string xpath)
        {
            xpath = xpath.Replace("{", "//*[@id='");
            xpath = xpath.Replace("}", "']");

            return xpath;
        }

        private static string idToXpath(string id, string xPath)
        {
            return "//*[@id='" + id + "']" + xPath;
        }

        //public static string GetInnerText(this DryHtmlDocument document, string id, string xpath)
        //{
        //    return SelectSingleNode(document, idToXpath(id, xpath)).InnerText;
        //}
        public static void SetHtml(this DryHtml document, string id, string xpath, string value)
        {
            SelectSingleNode(document, idToXpath(id, xpath)).InnerHtml = value;
        }


    }

    public class DryHtml
    {
        private HtmlDocument _template;

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
            _dom = template.TemplateReplace(model);
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

        public HtmlDocument HtmlDocument
        {
            get
            {
                return _template;
            }
            set
            {
                _template = value;
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
