using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WatchAuthApp.Startup))]
namespace WatchAuthApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
