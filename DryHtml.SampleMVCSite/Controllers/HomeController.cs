using DryHtml.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DryHtml.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return new Views.Readme(
                new ReadmeViewModel
                {
                    Title = "Title",
                    Header = "Header",
                    PartialHeader = "Partial header"
                });
        }
    }
}