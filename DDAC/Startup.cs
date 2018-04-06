using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DDAC.Startup))]
namespace DDAC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
