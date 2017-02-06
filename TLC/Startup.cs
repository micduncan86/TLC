using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TLC.Startup))]
namespace TLC
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
