using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CineExplorer.Startup))]
namespace CineExplorer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
