using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UnZipFileForWeb.Startup))]
namespace UnZipFileForWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
