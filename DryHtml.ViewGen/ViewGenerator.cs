using CsQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DryHtml.ViewGen
{

    /// <summary>
    /// Takes a prototype HTML and generates cshtml based on a ViewModel
    /// Described as custom attributes on a specified ViewModel class.
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public class ViewGenerator<TViewModel> : ViewGenerator
    {
        public ViewGenerator(string prototypeHtml) : base(prototypeHtml, typeof(TViewModel)) { }
        public ViewGenerator() : base(typeof(TViewModel)) { }
    }

    /// <summary>
    /// Takes a prototype HTML and generates cshtml based on a ViewModel
    /// Described as a string dictionary.
    /// </summary>
    public class ViewGenerator
    {
        CQ prototypeHtml;
        string csHtmlView;
        string selector;
        Type modelType;
        string outputCsHtmlViewFileName;
        Dictionary<string, string> modelDescriber;
        public ViewGenerator(string prototypeHtml, Dictionary<string, string> modelDescriber)
        {
            this.prototypeHtml = prototypeHtml;
            this.modelDescriber = modelDescriber;
        }
        public ViewGenerator(string prototypeHtml, string selector, Dictionary<string, string> modelDescriber)
        {
            this.prototypeHtml = prototypeHtml;
            this.modelDescriber = modelDescriber;
            this.selector = selector;
        }

        public ViewGenerator(Type modelType)
        {
            this.modelType = modelType;

            var modelAttribute = modelType.GetCustomAttribute<DryHtml.ViewModelGeneratorAttribute>();

            selector = modelAttribute.Selector;

            if (!string.IsNullOrEmpty(modelAttribute.FilenameOrUrl))
            {
                this.prototypeHtml = System.IO.File.ReadAllText(modelAttribute.FilenameOrUrl);
            }

            if (!string.IsNullOrEmpty(modelAttribute.GenerateViewFileName))
                outputCsHtmlViewFileName = modelAttribute.GenerateViewFileName;

            generateModelDescriberFromType();
        }

        void generateModelDescriberFromType()
        {
            modelDescriber = new Dictionary<string, string>();

            foreach (PropertyInfo pi in modelType.GetProperties())
            {

                var viewModelPropertyAttribute = pi.GetCustomAttribute<ViewModelPropertyAttribute>();
                if (viewModelPropertyAttribute != null)
                {
                    modelDescriber.Add(pi.Name, viewModelPropertyAttribute.Selector);
                }

            }
        }

        public ViewGenerator(string prototypeHtml, Type modelType)
        {
            this.prototypeHtml = prototypeHtml;

            this.modelType = modelType;

            var modelAttribute = modelType.GetCustomAttribute<DryHtml.ViewModelGeneratorAttribute>();

            selector = modelAttribute.Selector;

            generateModelDescriberFromType();


        }
        private void generateCshtml()
        {
            var csHtmlView = prototypeHtml;
            if (selector != "") csHtmlView = prototypeHtml.Select(selector);
            foreach (var property in modelDescriber)
            {
                csHtmlView[property.Value].Text("@Model." + property.Key);
            }

            var cshtmlViewString = new System.Text.StringBuilder();

            if (modelType != null)
            {
                cshtmlViewString.AppendFormat("@model {0}{1}", modelType.Name, Environment.NewLine);
            }
            cshtmlViewString.Append(csHtmlView.RenderSelection());

            this.csHtmlView = cshtmlViewString.ToString();


        }
        public string CsHtmlView
        {
            get
            {
                if (csHtmlView == null) generateCshtml();
                return csHtmlView;
            }
        }
    }
}
