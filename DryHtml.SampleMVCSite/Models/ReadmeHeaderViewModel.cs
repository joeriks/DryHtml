using DryHtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HtmlViewEngine.Models
{
    [ViewModelGenerator("Prototypes/SamplePrototype.html", "#header", "Views/ReadmeHeaderViewModel.cshtml")]
    public class ReadmeHeaderViewModel
    {

        [ViewModelProperty("h1")]
        public string Header { get; set; }
        [ViewModelProperty("p")]
        public string Description { get; set; }

    }
}