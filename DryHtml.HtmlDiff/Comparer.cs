using CsQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryHtml.HtmlDiff
{
    public class Comparer
    {
        private string compareHtml;
        private string compareToHtml;
        private string selector;

        public Comparer(string compareHtml, string compareToHtml, string selector = "*")
        {
            this.compareHtml = compareHtml;
            this.compareToHtml = compareToHtml;
            this.selector = selector;
            compare();
        }

        private bool compare()
        {
            if (selector == "") selector = "*";
            var compareHtmlCq = ((CQ)compareHtml).Select(selector).Get(0);
            var compareToHtmlCq = ((CQ)compareToHtml).Select(selector).Get(0);
            return TraverseNode(compareHtmlCq, compareToHtmlCq, "");
        }

        public List<Diff> Diffs = new List<Diff>();
        public struct Diff
        {
            public string selector;
            public bool isText;
            public string compareSourceHtml;
            public string compareWithHtml;
        }

        bool CompareNodes(IDomObject n, IDomObject compareToNode, string path)
        {

            var outerHtml = n.Render();
            var compareToOuterHtml = (compareToNode == null) ? "" : compareToNode.Render();

            var getAttributes = new Func<IDomObject, string>(f => (f.Attributes == null) ? "" : string.Join(",", f.Attributes.Select(a => a.Key + "=" + a.Value)));

            var thisNodeEquals = (compareToNode != null
                && n.NodeName == compareToNode.NodeName
                && n.NodeType == compareToNode.NodeType
                && n.NodeValue == compareToNode.NodeValue)
                && getAttributes(n) == getAttributes(compareToNode);


            var nodeHtmlEquals = (compareToNode != null && outerHtml == compareToOuterHtml);

            var pathAttributes = "";
            if (n.Attributes != null)
            {
                var nSelectorAttributes = n.Attributes.Where(a => a.Key == "id" || a.Key == "class");
                if (nSelectorAttributes.Any())
                {
                    var idAttribute = nSelectorAttributes.Where(sa => sa.Key == "id");
                    var classAttribute = nSelectorAttributes.Where(sa => sa.Key == "class");

                    if (idAttribute.Any()) pathAttributes = "#" + idAttribute.First().Value;
                    else
                        if (classAttribute.Any()) pathAttributes = "." + classAttribute.First().Value;

                }
            }
            path = path + ">" + n.NodeName + pathAttributes + "(" + n.Index.ToString() + ")";

            if (thisNodeEquals)
            {
                if (n.HasChildren && compareToNode != null)
                {
                    return TraverseNode(n, compareToNode, path);
                }
                return true;

            }
            else
            {

                var p = n;
                var pp = "";
                var breakNow = false;

                var diff = new Diff();

                do
                {

                    if (p != null && p.Attributes != null)
                    {
                        var idAttr = p.Attributes.Where(a => a.Key == "id");
                        if (idAttr.Any())
                        {
                            pp = p.NodeName + "#" + idAttr.First().Value + " " + pp;
                            breakNow = true;
                        }
                    }
                    var hasClassAttribute = false;
                    if (!breakNow)
                    {
                        if (p != null && p.Attributes != null)
                        {
                            var idCssClass = p.Attributes.Where(a => a.Key == "class");
                            if (idCssClass.Any())
                            {
                                hasClassAttribute = true;
                                pp = p.NodeName.ToLower() + "." + idCssClass.First().Value.Replace(" ", ".") + ((pp!="")?" > " + pp:"");
                            }
                        }
                    }
                    if (!breakNow && !hasClassAttribute)
                    {
                        if (p.NodeType == NodeType.TEXT_NODE) 
                            diff.isText = true;
                        if (p.NodeType == NodeType.ELEMENT_NODE)
                        {
                            var indexSelector = "";
                            if (p.ParentNode.ChildNodes.Count(t=>t.NodeName==p.NodeName)> 1) indexSelector = ":nth-child(" + (p.Index+1).ToString() + ")";
                            pp = p.NodeName.ToLower() + indexSelector + ((pp != "") ? " > " + pp : ""); //(" + p.Index.ToString() + ")
                        }
                    }

                    p = p.ParentNode;
                }
                while (!(p == null || breakNow));

                diff.selector = pp;
                diff.compareSourceHtml = outerHtml;
                diff.compareWithHtml = compareToOuterHtml;
                Diffs.Add(diff);

                return false;

            }

        }

        bool TraverseNode(IDomObject node, IDomObject compareToNode, string path)
        {
            var result = true;
            if (node != null && node.HasChildren)
                foreach (IDomObject n in node.ChildNodes)
                {

                    var compareToMissing = (compareToNode == null || !compareToNode.HasChildren || n.Index >= compareToNode.ChildNodes.Length);

                    IDomObject compareToNodeChild = null;
                    if (!compareToMissing) compareToNodeChild = compareToNode.ChildNodes[n.Index];
                    var childResult = CompareNodes(n, compareToNodeChild, path);
                    if (!childResult) result = false;
                }
            return result;
        }


    }

}
