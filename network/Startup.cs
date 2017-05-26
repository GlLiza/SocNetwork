using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(network.Startup))]
namespace network
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
