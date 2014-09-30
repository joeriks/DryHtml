using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.IO;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(DryHtml.SiteTools.SignalRApp.Startup1))]

namespace DryHtml.SiteTools.SignalRApp
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();

            app.Run(context =>
            {
                if (context.Request.Uri.AbsolutePath == "/jquery.js")
                {
                    using (var jqueryStream = typeof(Startup1).Assembly.GetManifestResourceStream("DryHtml.SiteTools.SignalRApp.Scripts.jquery-1.6.4.min.js"))
                    {
                        context.Response.ContentType = "text/javascript";
                        using (var memoryStream = new MemoryStream())
                        {
                            jqueryStream.CopyTo(memoryStream);
                            return context.Response.WriteAsync(memoryStream.ToArray());
                        }

                    }

                }
                else if (context.Request.Uri.AbsolutePath == "/signalr.js")
                {
                    using (var jqueryStream = typeof(Startup1).Assembly.GetManifestResourceStream("DryHtml.SiteTools.SignalRApp.Scripts.jquery.signalR-2.1.2.min.js"))
                    {
                        context.Response.ContentType = "text/javascript";
                        using (var memoryStream = new MemoryStream())
                        {
                            jqueryStream.CopyTo(memoryStream);
                            return context.Response.WriteAsync(memoryStream.ToArray());
                        }

                    }
                }
                else
                {

                    context.Response.ContentType = "text/html";
                    var button = new {id="btn",label="hello", invoke="hello"}.Format("<button id='{id}' onclick='alert(myHub.server.send(\"{invoke}\",\"yyy\"))'>{label}</button>");
                    HubFuncs.Funcs = new System.Collections.Generic.Dictionary<string, Func<string, string>>();
                    HubFuncs.Funcs.Add("hello", s => s.ToUpper());

                    return context.Response.WriteAsync(@"
<html>
    <head>
        <script src='/jquery.js'></script>
        <script src='/signalr.js'></script>
        <script src='/signalr/hubs'></script>
        <script>$(function(){
                    window.myHub = $.connection.myHub;
                    $.connection.hub.start();
                });
        </script>
    </head>
    <body>" + button + @"</body>
</html>
");
                }
            });
        }
    }
}
