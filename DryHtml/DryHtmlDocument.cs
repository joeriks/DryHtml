using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryHtml
{
    public static class DryHtmlExtensions
    {
        // @header/h1 --> //[@id='header']/h1
        public static HtmlNode SelectSingleNode(this DryHtmlDocument document, string xpath)
        {
            //if (dataDryHtml != "")
            //{
            //    xpath = "//*[@data-dryhtml='" + dataDryHtml + "']" + xpath;
            //}

            return document.HtmlDocument.DocumentNode.SelectSingleNode(xpath);
        }

        public static string GetInnerText(this DryHtmlDocument document, string xpath)
        {
            xpath = xPathId(xpath);
            return SelectSingleNode(document, xpath).InnerText;
        }
        public static void SetHtml(this DryHtmlDocument document, string xpath, string value)
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
        public static void SetHtml(this DryHtmlDocument document, string id, string xpath, string value)
        {
            SelectSingleNode(document, idToXpath(id, xpath)).InnerHtml = value;
        }


    }

    public class DryHtmlDocument
    {
        private HtmlDocument _template;

        public DryHtmlDocument(string templateFilePath = "")
        {

            _template = new HtmlDocument();

            if (templateFilePath.Length > 255 || templateFilePath.Contains('<'))
            {
                _template.LoadHtml(templateFilePath);
            } else if (templateFilePath != "")
                _template.Load(templateFilePath);
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
        public String Html
        {
            get
            {
                return _template.DocumentNode.OuterHtml;
            }
            set
            {
                _template.LoadHtml(value);
            }
        }



    }
}
