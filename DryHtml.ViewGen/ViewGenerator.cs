using CsQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using DryHtml.ViewExtractor;

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

        public string OutputRootPath { get; set; }

        CQ prototypeHtml;
        string csHtmlView;
        string selector;
        Type modelType;
        string outputCsHtmlViewFileName;
        Dictionary<string, string> modelDescriber;

        private string csModel;
        private string modelName;

        public PrototypeExtractor PrototypeExtractor { get; set; }

        List<ViewGenOptions> ViewGenList { get; set; }
        public class PropertyOptions
        {
            public string Name { get; set; }
            public string Selector { get; set; }
            public string Type { get; set; } // ""=text, "Tag"
        }

        public class ViewGenOptions
        {
            public ViewModelOptions ViewModel { get; set; }
            public List<PropertyOptions> Properties { get; set; }
            public OutputOptions Output { get; set; }
            public class ViewModelOptions
            {
                public string Prototype { get; set; }
                public string Selector { get; set; }
                public string Name { get; set; }
            }
            public class OutputOptions
            {
                public string View { get; set; }
                public string ViewModel { get; set; }
            }
        }

        public ViewGenerator(string jsonViewGenOptionsFileName)
        {
            var viewGenOptionsFile = System.IO.File.ReadAllText(jsonViewGenOptionsFileName);
            ViewGenList = JsonConvert.DeserializeObject<List<ViewGenOptions>>(viewGenOptionsFile);

        }

        public void GenerateAll()
        {
            foreach (var x in ViewGenList)
            {
                var prototypeHtml = System.IO.File.ReadAllText(x.ViewModel.Prototype);
                var modelDescriber = x.Properties.ToDictionary(p => p.Name, p => p.Selector);
                var viewGen = new ViewGenerator(prototypeHtml, x.ViewModel.Selector, modelDescriber, x.ViewModel.Name);

                if (!string.IsNullOrEmpty(x.Output.View))
                {
                    System.IO.File.WriteAllText(OutputRootPath + "/" + x.Output.View, viewGen.CsHtmlView);
                }
                if (!string.IsNullOrEmpty(x.Output.ViewModel))
                {
                    System.IO.File.WriteAllText(OutputRootPath + "/" + x.Output.ViewModel, viewGen.CsModel);
                }

            }
        }
        public ViewGenerator(string prototypeHtml, string selector, Action<ViewGenerator> customGenerator)
        {
            this.prototypeHtml = prototypeHtml;
            this.selector = selector;
            customGenerator(this);
        }
        public ViewGenerator(string prototypeHtml, Dictionary<string, string> modelDescriber)
        {
            this.prototypeHtml = prototypeHtml;
            this.modelDescriber = modelDescriber;
        }
        public ViewGenerator(string prototypeHtml, string selector, Dictionary<string, string> modelDescriber, string modelName = "")
        {
            this.prototypeHtml = prototypeHtml;
            this.modelDescriber = modelDescriber;
            this.selector = selector;

            this.modelName = modelName;
        }
        public ViewGenerator(string prototypeHtml, PrototypeExtractor prototypeExtractor)
        {
            this.prototypeHtml = prototypeHtml;
            this.PrototypeExtractor = prototypeExtractor;
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
                if (csHtmlView == null)
                {
                    if (this.PrototypeExtractor != null)
                    {
                        helpers = new Dictionary<string, string>();
                        generateCsHtmlFromPrototypeExtractor(this.PrototypeExtractor, this.prototypeHtml);
                        var code = new StringBuilder();
                        foreach (var helper in helpers)
                        {
                            code.AppendLine(helper.Value);                            
                        }
                        csHtmlView = code.ToString();
                    }
                    else generateCshtml();
                }
                return csHtmlView;
            }
        }

        private Dictionary<string, string> helpers;

        private string generateCsHtmlFromPrototypeExtractor(ViewExtractor.PrototypeExtractor prototypeExtractor, CQ outerCQ, string helperPrefix = "", string outerSelector = "")
        {
            var innerCq = outerCQ.Select(outerSelector + " " + prototypeExtractor.Selector + " " + prototypeExtractor.ValueSelector);

            foreach (var prop in prototypeExtractor.ChildExtractors)
            {

                var outerHtmlCq = outerCQ.Select(outerSelector + " " + prototypeExtractor.Selector + " " + prototypeExtractor.ValueSelector + " " + prop.Selector);

                if (prop.ChildExtractors.Any())
                    generateCsHtmlFromPrototypeExtractor(prop, outerHtmlCq, helperPrefix + "_" + prop.Name + "_", outerSelector + " " + prototypeExtractor.Selector + " " + prototypeExtractor.ValueSelector);

                var code = new StringBuilder();
                var helperName = helperPrefix + prop.Name;
                var helperSignature = helperName + "(" + prop.Type + " " + prop.Name + ")";
                code.AppendLine("@helper " + helperSignature + " {");

                var valueCode = "@" + prop.Name + "";

                if (prop.ChildExtractors.Any())
                {
                    var childRender = new StringBuilder();
                    foreach (var child in prop.ChildExtractors)
                    {
                        childRender.AppendLine("@" + helperName + "_" + child.Name + "(" + prop.Name + "." + child.Name + ")");
                    }
                    valueCode = childRender.ToString();
                    
                }

                if (prop.ValueSelector == "#text")
                    outerHtmlCq.Select(outerSelector + " " + prototypeExtractor.Selector + " "  +prototypeExtractor.ValueSelector + " " + prop.Selector).Get(0).ChildNodes.Where(t=>t.NodeType == NodeType.TEXT_NODE && !string.IsNullOrWhiteSpace(t.NodeValue)).FirstOrDefault().NodeValue = valueCode;
                else
                    outerHtmlCq.Select(outerSelector + " " + prototypeExtractor.Selector + " "  +prototypeExtractor.ValueSelector + " " + prop.Selector + " " + prop.ValueSelector).First().Html(valueCode);



                code.AppendLine(outerHtmlCq.First().RenderSelection());
                code.AppendLine("}");

                helpers.Add(helperSignature, code.ToString());

            }

            

            return innerCq.RenderSelection();

        }

        public string CsModel
        {
            get
            {
                if (csModel == null)
                {
                    if (this.PrototypeExtractor != null)
                    {
                        csModel = generateCsModelFromPrototypeExtractor(this.PrototypeExtractor);
                    }
                    else generateCsModel();
                }
                return csModel;
            }
        }

        private string generateCsModelFromPrototypeExtractor(PrototypeExtractor prototypeExtractor, int tabLevel = 0)
        {
            var generatedCode = new StringBuilder();

            generatedCode.AppendFormat(new string(' ', tabLevel * 2) + "public partial class {0} {{", prototypeExtractor.Name).AppendLine();
            foreach (var prop in prototypeExtractor.ChildExtractors)
            {
                if (prop.ChildExtractors.Any())
                    generatedCode.Append(generateCsModelFromPrototypeExtractor(prop, tabLevel + 1));

                generatedCode.AppendFormat(new string(' ', (1 + tabLevel) * 2) + "public {0} {1} {{get;set;}}", prop.Type, prop.Name).AppendLine();
            }

            generatedCode.AppendLine(new string(' ', tabLevel * 2) + "}");

            return generatedCode.ToString();

        }

        private void generateCsModel()
        {
            var generatedCode = new StringBuilder();

            generatedCode.AppendFormat("public partial class {0} {{", modelName).AppendLine();

            foreach (var prop in modelDescriber)
            {
                generatedCode.AppendFormat("    public {0} {1} {{get;set;}}", "string", prop.Key).AppendLine();
            }

            generatedCode.AppendLine("}");


            this.csModel = generatedCode.ToString();


        }
    }
}
