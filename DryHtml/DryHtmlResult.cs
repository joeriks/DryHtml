﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DryHtml
{
    public class DryHtmlResult : ContentResult
    {
        private DryHtml htmlDocument;

        public CsQuery.CQ DOM
        {
            get
            {
                return htmlDocument.DOM;
            }
            set
            {
                htmlDocument.DOM = value;
            }
        }
    

        public DryHtml DryHtmlDocument
        {
            get { return htmlDocument; }
            set { htmlDocument = value; }
        }

        public DryHtmlResult(string templateFilePath)
        {

            htmlDocument = new DryHtml(System.Web.HttpContext.Current.Server.MapPath(templateFilePath));

        }
        public override void ExecuteResult(ControllerContext context)
        {
            base.Content = htmlDocument.Html.ToString();
            base.ExecuteResult(context);
        }
    }
}
