using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryHtml.SiteTools.SignalRApp
{
    public static class HubFuncs{

        public static Dictionary<string, Func<string, string>> Funcs { get; set; }
    
    }

    public class MyHub : Hub
    {
        

        public MyHub(){
        }
        public void Send(string name, string message)
        {
            HubFuncs.Funcs[name](message);
        }
    }
}
