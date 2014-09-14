using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
public static class TemplateReplaceExtension
{
    /// <summary>
    /// Returns a new string where all placeholders "{a} {b}" are replaced with the anonymous object properties with the same names ( new {a,b} )
    /// </summary>
    /// <param name="template"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static string Replace(this string template, object parameters)
    {
        var parametersDictionary = parameters.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(parameters, null).ToString());
        return Regex.Replace(template, @"\{(.+?)\}", m => parametersDictionary[m.Groups[1].Value]);
    }

    public static IHtmlString Concat(this IHtmlString firstString, IHtmlString secondString)
    {
        return new HtmlString(firstString.ToString() + "\r\n" + secondString.ToString());
    }
}
