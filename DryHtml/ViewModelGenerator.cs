using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DryHtml
{
    public class ViewModelGeneratorAttribute : Attribute
    {
        public string Selector {get;set;}
        public string Resource { get; set; }

        public ViewModelGeneratorAttribute(string selector)
        {
            Selector = selector;
        }
        public ViewModelGeneratorAttribute(string resource, string selector)
        {
            Selector = selector;
            Resource = resource;
        }
    }
}
