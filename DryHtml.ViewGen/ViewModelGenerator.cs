using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DryHtml
{
    public class ViewModelPropertyAttribute : Attribute
    {
        public string Selector { get; set; }

        public ViewModelPropertyAttribute(string selector)
        {
            Selector = selector;
        }
    }

    public class ViewModelGeneratorAttribute : Attribute
    {
        public string Selector { get; set; }
        public string FilenameOrUrl { get; set; }
        public string GenerateViewFileName { get; set; }

        public ViewModelGeneratorAttribute(string selector)
        {
            Selector = selector;
        }
        public ViewModelGeneratorAttribute(string filenameOrUrl, string selector)
        {
            Selector = selector;
            FilenameOrUrl = filenameOrUrl;
        }
        public ViewModelGeneratorAttribute(string filenameOrUrl, string selector, string generateViewFileName)
        {
            Selector = selector;
            FilenameOrUrl = filenameOrUrl;
            GenerateViewFileName = generateViewFileName;
        }
    }

}
