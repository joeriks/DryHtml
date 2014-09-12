using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DryHtml.Startup))]
namespace DryHtml
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
         
        }
    }
}
