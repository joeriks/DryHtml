using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Linq;
using System.Collections.Generic;
public static class TemplateReplaceExtension
{
    /// <summary>
    /// Returns a new string where all placeholders "{a} {b}" are replaced with the anonymous object properties with the same names ( new {a,b} )
    /// </summary>
    /// <param name="template"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static string TemplateReplace(this string template, object parameters)
    {
        return Regex.Replace(template, @"\{(.+?)\}", m => string.Concat((dynamic)(parameters.GetType().IsValueType ? parameters : parameters.GetType().GetProperty(m.Groups[1].Value).GetValue(parameters) ?? "")));
    }

    public static bool IsSimpleType(
        this Type type)
    {
        return
            type.IsPrimitive ||
            new Type[] { 
				typeof(String),
				typeof(Decimal),
				typeof(DateTime),
				typeof(DateTimeOffset),
				typeof(TimeSpan),
				typeof(Guid)
			}.Contains(type);

        // ||
        //    Convert.GetTypeCode(type) != TypeCode.Object
    }
    public static string Format<T1, T2, T3>(this T1 model, Func<T1, T2> map, Func<T2, T3> secondMap, string template)
    {
        return secondMap(map(model)).Format(template);
    }
    public static string Format<TIn, TOut>(this TIn model, Func<TIn, TOut> map, string template)
    {
        return map(model).Format(template);
    }

    public struct TypeTemplate
    {
        public Type Type { get; set; }
        public string Template { get; set; }
    }
    public static string Format(this object model, string template, params TypeTemplate[] templates)
    {
        //var replacePattern = @"\{(.+?)\}";
        //return string.Format(template, model);

        if (model == null) return null;

        if (!(model is IList))
            if (template == null)
            {
                template = templates.LastOrDefault(t => t.Type == model.GetType()).Template;
                if (template == null) template = "{0}";
            }
            else
            {
                templates = templates.ToList().Concat(new List<TypeTemplate>() { new TypeTemplate { Type = model.GetType(), Template = template } }).ToArray();
            }

        if (IsSimpleType(model.GetType())) return string.Format(template, model);


        if (model is IList)
        {
            var mE = (IEnumerable)model;
            var result = "";
            foreach (var n in mE)
            {

                //
                // if the model is a list then use the template for each item in the list
                //
                result += n.Format(template, templates);
            }
            return result;
        }
        else
        {
            var result = ParenthesisContentReplacer(template, m =>
            {

                var split = m.Split(new char[] { ':' }, 2);
                var value = model.GetType().GetProperty(split[0]).GetValue(model);
                if (value == null) return null;
                if (split.Length > 1)
                {
                    // 
                    // if included template for the model, then use that one
                    //
                    return value.Format(split[1], templates);
                }
                else
                {
                    var preferredFormatter = templates.LastOrDefault(t => t.Type == value.GetType());
                    var preferredFormat = preferredFormatter.Template;
                    return value.Format(preferredFormat, templates);
                    //return string.Format("{0}", value);
                }

            });
            //var result = Regex.Replace(template, @"\{(.+?)\}", m => string.Concat((dynamic)(model.GetType().IsValueType ? model : model.GetType().GetProperty(m.Groups[1].Value).GetValue(model) ?? "")));
            return result;
        }
    }
    public static string Format<T>(this Dictionary<string, T> model, string template)
    {
        var list = model.ToList();// {key,value}{key,value}
        return list.Format(template);
    }
    public static string Format<T>(this List<T> model, string template)
    {
        var result = string.Concat(model.Select<T, string>(m => (((object)m).Format(template))));
        return result;
    }

    //    }
    //if (model.GetType().Is(typeof(IDictionary)))
    //{
    //    var dictionary = (IDictionary)model;

    //    foreach (KeyValuePair<object, object> kv in dictionary)
    //    {

    //    }

    //    return dictionary.ToList().Format(template);
    //}
    //var getDictionaryValue = new Func<string, object>(m => ((IDictionary)model)[m]);
    //var getPropertyValue = new Func<string, object>(m => model.GetType().GetProperty(m).GetValue(model) ?? "");
    //var getValue = new Func<Match, dynamic>(m => isValueType ? model : getPropertyValue(m.Groups[1].Value));
    //var me = new MatchEvaluator(m => string.Concat(getValue(m)));

    //return Regex.Replace(template, replacePattern, me);


    public static string ToString<T>(this T model, Func<T, string> replacer)
    {
        return replacer(model);
    }

    //public static IHtmlString Concat(this IHtmlString firstString, IHtmlString secondString)
    //{
    //    return new HtmlString(firstString.ToString() + "\r\n" + secondString.ToString());
    //}

    struct Replacer
    {
        public int startPosition;
        public int endPosition;
    }

    public static string ParenthesisContentReplacer(string s, Func<string, string> matchReplacer)
    {

        var currentStartParentesisPosition = -1;
        var currentParentesisLevel = 0;
        var newString = s;

        var replacers = new List<Replacer>();

        for (int i = 0; i < s.Length; i++)
        {
            switch (s[i])
            {
                case '{':
                    currentParentesisLevel += 1;
                    if (currentParentesisLevel == 1) currentStartParentesisPosition = i;
                    break;
                case '}':
                    currentParentesisLevel -= 1;
                    if (currentParentesisLevel < 0) throw new Exception("Unmatched parens");
                    if (currentParentesisLevel == 0)
                        replacers.Add(new Replacer { startPosition = currentStartParentesisPosition, endPosition = i });

                    break;
                default:

                    break;
            }

        }

        if (currentParentesisLevel > 0) throw new Exception("Unmatched parens");
        var sNew = s;
        foreach (var element in replacers)
        {
            var token = s.Substring(element.startPosition + 1, element.endPosition - element.startPosition - 1);
            var newValue = matchReplacer(token);
            sNew = sNew.Replace("{" + token + "}", newValue);
        }

        return sNew;

    }
}
