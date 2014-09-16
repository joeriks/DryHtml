using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TransformCodeGenerator;

namespace DryHtml.VisualStudioCustomTool
{
    [ComVisible(true)]
    [Guid("67B22DD9-A705-4EAA-B4E4-3345FE9837E9")]
    //[CodeGeneratorRegistration(typeof(ToUppercase), "Uppercasification!", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true)]
    [CustomTool("ViewGen")]
    public class ViewGen:CustomToolBase
    {
        protected override string DefaultExtension()
        {
            return ".cshtml";
        }

        protected override byte[] Generate(string inputFilePath, string inputFileContents, string defaultNamespace, IVsGeneratorProgress progressCallback)
        {

            var viewGen = new DryHtml.ViewGen.ViewGenerator(inputFilePath);
            viewGen.GenerateAll();

            progressCallback.Progress(100, 100);

            return new byte[0];
        }
    }
}
