using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DryHtml
{
    public static class TemplateReplaceExtension
    {
        public static string TemplateReplace(this string template, object parameters)
        {
            var parametersDictionary = parameters.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(parameters, null).ToString());
            return Regex.Replace(template, @"\{(.+?)\}", m => parametersDictionary[m.Groups[1].Value]);
        }
    }
}
