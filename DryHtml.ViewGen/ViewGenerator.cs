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
        public class Extractor
        {
            public string Name { get; set; }
            public string Selector { get; set; }
            public string ValueSelector { get; set; }
            public string Type { get; set; } // ""=text, "Tag"
            public List<Extractor> Extractors { get; set; }
        }
        public class ViewGenOptions
        {
            public ViewModelOptions ViewModel { get; set; }
            public List<PropertyOptions> Properties { get; set; }
            public List<Extractor> Extractors { get; set; }
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

        public void addFromExtractors(PrototypeExtractor parent, List<Extractor> ex)
        {

            foreach (var ch in ex)
            {
                if (ch.Selector == ":next-child")
                {
                    ch.Selector = ">:nth-child(" + (parent.ChildExtractors.Count() + 1).ToString() + ")";
                }
                if (string.IsNullOrEmpty(ch.Type))
                    ch.Type = "string";

                var pr = new PrototypeExtractor
                {
                    Parent = parent,
                    Name = ch.Name,
                    Selector = ch.Selector,
                    ValueSelector = ch.ValueSelector,
                    Type = ch.Type,
                    WrapNullCheck = true
                };
                if (ch.Extractors != null)
                {
                    addFromExtractors(pr, ch.Extractors);
                }
                parent.ChildExtractors.Add(pr);

            }

        }

        public void GenerateAll()
        {
            foreach (var x in ViewGenList)
            {
                var prototypeHtml = System.IO.File.ReadAllText(x.ViewModel.Prototype);
                ViewGenerator viewGen = null;
                if (x.Properties != null)
                {
                    var modelDescriber = x.Properties.ToDictionary(p => p.Name, p => p.Selector);
                    viewGen = new ViewGenerator(prototypeHtml, x.ViewModel.Selector, modelDescriber, x.ViewModel.Name);
                }
                else
                {
                    var prototypeExtractor = new PrototypeExtractor
                    {
                        Name = x.ViewModel.Name,
                        Selector = x.ViewModel.Selector,
                        ValueSelector = ""
                    };

                    addFromExtractors(prototypeExtractor, x.Extractors);

                    viewGen = new ViewGenerator(prototypeHtml, prototypeExtractor);

                }

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
                        var mainCode = generateCsHtmlFromPrototypeExtractor(this.PrototypeExtractor, this.prototypeHtml);
                        var code = new StringBuilder();

                        code.AppendLine("@model " + this.PrototypeExtractor.Name);

                        foreach (var helper in helpers)
                        {
                            code.AppendLine(helper.Value);
                        }
                        code.AppendLine(mainCode);
                        csHtmlView = code.ToString();
                    }
                    else generateCshtml();
                }
                return csHtmlView;
            }
        }

        private Dictionary<string, string> helpers;

        private string generateCsHtmlFromPrototypeExtractor(ViewExtractor.PrototypeExtractor prototypeExtractor, CQ outerCQ, string helperPrefix = "", string outerSelector = "", string modelPrefix ="Model.")
        {
            var innerCq = outerCQ.Select(outerSelector + " " + prototypeExtractor.Selector + " " + prototypeExtractor.ValueSelector).First();
            var wrapNullCheck = true;
            foreach (var prop in prototypeExtractor.ChildExtractors)
            {

                var firstSelector = (prototypeExtractor.Type != null && prototypeExtractor.Type.StartsWith("List")) ? ":first" : "";

                var outerHtmlCq = outerCQ.Select(outerSelector + " " + prototypeExtractor.Selector + " " + prototypeExtractor.ValueSelector + firstSelector + " " + prop.Selector);

                if (prop.ChildExtractors.Any())
                    generateCsHtmlFromPrototypeExtractor(prop, outerHtmlCq, helperPrefix + prop.Name + "_", outerSelector + " " + prototypeExtractor.Selector + " " + prototypeExtractor.ValueSelector + firstSelector);

                var code = new StringBuilder();
                var helperName = helperPrefix + prop.Name;
                var helperSignature = helperName + "(" + prop.Type + " " + prop.Name + ")";
                code.AppendLine("@helper " + helperSignature + " {");
                if (prop.WrapNullCheck) code.AppendLine("if (" + prop.Name + "!=null){");

                var valueCode = "@" + prop.Name;

                if (prototypeExtractor.Parent == null)
                    valueCode = "@" + modelPrefix + prop.Name;

                if (prop.ChildExtractors.Any())
                {
                    var childRender = new StringBuilder();
                    foreach (var child in prop.ChildExtractors)
                    {
                        childRender.AppendLine("@" + helperName + "_" + child.Name + "(item." + child.Name + ")");
                    }
                    valueCode = childRender.ToString();

                }

                if (prop.ValueSelector == "#text")
                    outerHtmlCq.Select(outerSelector + " " + prototypeExtractor.Selector + " " + prototypeExtractor.ValueSelector + " " + prop.Selector).Get(0).ChildNodes.Where(t => t.NodeType == NodeType.TEXT_NODE && !string.IsNullOrWhiteSpace(t.NodeValue)).FirstOrDefault().NodeValue = valueCode;
                else
                {
                    if (prop.Type.StartsWith("List"))
                    {
                        outerHtmlCq.Select(outerSelector + " " + prototypeExtractor.Selector + " " + prototypeExtractor.ValueSelector + " " + prop.Selector + " " + prop.ValueSelector).Html(valueCode);
                        outerHtmlCq.Select(outerSelector + " " + prototypeExtractor.Selector + " " + prototypeExtractor.ValueSelector + " " + prop.Selector + " " + prop.ValueSelector).Each((i, d) =>
                        {
                            if (i > 0) d.OuterHTML = "";
                        });

                        var innerHtml = outerHtmlCq.Select(outerSelector + " " + prototypeExtractor.Selector + " " + prototypeExtractor.ValueSelector + " " + prop.Selector + " " + prop.ValueSelector).First().RenderSelection();

                        var listCode = new StringBuilder();
                        if (prop.WrapNullCheck) code.AppendLine("if (" + modelPrefix + prop.Name + "!=null){");
                        listCode.AppendLine("@foreach (var item in " + modelPrefix + prop.Name + ") {");
                        listCode.AppendLine(innerHtml);
                        listCode.AppendLine("}");
                        if (prop.WrapNullCheck) code.AppendLine("}");

                        outerHtmlCq.Select(outerSelector + " " + prototypeExtractor.Selector + " " + prototypeExtractor.ValueSelector + " " + prop.Selector + " " + prop.ValueSelector)[0].OuterHTML=listCode.ToString();

                    }
                    else
                    {
                        outerHtmlCq.Select(outerSelector + " " + prototypeExtractor.Selector + " " + prototypeExtractor.ValueSelector + " " + prop.Selector + " " + prop.ValueSelector).Html(valueCode);
                    }
                }

                code.AppendLine(outerHtmlCq.RenderSelection());
                code.AppendLine("}");
                if (prop.WrapNullCheck) code.AppendLine("}");

                if (prototypeExtractor.Parent!=null) helpers.Add(helperSignature, code.ToString());

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

            var typeName = prototypeExtractor.Name;
            if (prototypeExtractor.Type !=null && prototypeExtractor.Type.StartsWith("List"))
            {
                typeName = prototypeExtractor.Type.Substring(5, prototypeExtractor.Type.Length - 6);
            }

            generatedCode.AppendFormat(new string(' ', tabLevel * 2) + "public partial class {0} {{", typeName).AppendLine();
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
