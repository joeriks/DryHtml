using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryHtml
{
    public abstract class TemplatedViewModel
    {
        public abstract string Template();
        public override string ToString()
        {
            return TemplateReplaceExtension.TemplateReplace(Template(), this);
        }
    }
    public abstract class TemplatedViewModel<TModel>
    {
        public TModel model { get; set; }
        public abstract string Template();
        public override string ToString()
        {
            return TemplateReplaceExtension.TemplateReplace(Template(), model);
        }
    }
}
