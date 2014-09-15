using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DryHtml
{
    public class ViewModelSelectorPropertyAttribute : Attribute
    {
        public string Selector { get; set; }

        public ViewModelSelectorPropertyAttribute(string selector)
        {
            Selector = selector;
        }
    }

    public class ViewModelSelectorAttribute : Attribute
    {
        public string Selector { get; set; }
        public string FilenameOrUrl { get; set; }

        public ViewModelSelectorAttribute(string selector)
        {
            Selector = selector;
        }
        public ViewModelSelectorAttribute(string filenameOrUrl, string selector)
        {
            Selector = selector;
            FilenameOrUrl = filenameOrUrl;
        }
    }

}
