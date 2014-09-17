using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryHtml.ViewExtractor
{
    public class PrototypeExtractor
    {
        public PrototypeExtractor Parent { get; set; }
        public List<PrototypeExtractor> ChildExtractors { get; set; }
        public string Name { get; set; }
        public string Selector { get; set; }
        public string ValueSelector { get; set; }
        public bool WrapNullCheck { get; set; }


        public PrototypeExtractor()
        {
            ChildExtractors = new List<PrototypeExtractor>();
        }
        public PrototypeExtractor(string name, string selector, string valueSelector, Action<PrototypeExtractor> self)
        {
            Name = name;
            Selector = selector;
            ValueSelector = ValueSelector;
            ChildExtractors = new List<PrototypeExtractor>();
            self(this);
        }
        public PrototypeExtractor(string name, string selector, string valueSelector)
        {
            ChildExtractors = new List<PrototypeExtractor>();
        }
        public PrototypeExtractor(PrototypeExtractor parent, string name, string selector, string valueSelector)
        {
            ChildExtractors = new List<PrototypeExtractor>();
        }
        public PrototypeExtractor AddChildAt(string name, string selector, string valueSelector, string type, Action<PrototypeExtractor> self = null)
        {
            var pe = new PrototypeExtractor { Name = name, Selector = selector, ValueSelector = valueSelector, Type = type, WrapNullCheck = true, Parent = this };
            if (self != null) self(pe);
            ChildExtractors.Add(pe);
            return this;
        }
        public PrototypeExtractor AddChildNext(string name, string valueSelector, Action<PrototypeExtractor> self = null)
        {
            return AddChildAt(name, ">:nth-child(" + (ChildExtractors.Count() + 1) + ")", valueSelector, "string", self);
        }

        public PrototypeExtractor AddChild(PrototypeExtractor propertyExtractor, Action<PrototypeExtractor> add = null)
        {
            propertyExtractor.Parent = this;
            ChildExtractors.Add(propertyExtractor);
            return this;
        }

        public string Type { get; set; }

    }

}
