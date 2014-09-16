using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DryHtml.ToolsMVCSite.Controllers
{
    public class SiteCompareController : ApiController
    {

        public class SiteCompareControllerPostModel
        {
            public string url1 { get; set; }
            public string url2 { get; set; }
            public string selector { get; set; }
        }
        public object Post([FromBody]SiteCompareControllerPostModel model)
        {

            var siteCompare = new DryHtml.SiteDiff.SiteCompare(model.url1, model.url2, model.selector);
            return siteCompare.CompareResult;
        }

    }
}
