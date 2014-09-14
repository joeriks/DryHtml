using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsQuery;
namespace DryHtml.Tests
{
    class HtmlComparer
    {
        private string compareSource;
        private string compareWith;

        public HtmlComparer(string compareSource, string compareWith)
        {
            // TODO: Complete member initialization
            this.compareSource = compareSource;
            this.compareWith = compareWith;
        }
        internal object Result()
        {
            var s = new CQ(compareSource);
            var w = new CQ(compareWith);

            

            throw new NotImplementedException();
        }
    }
}
