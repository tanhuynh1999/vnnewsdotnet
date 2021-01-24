using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(vnnews.Startup))]
namespace vnnews
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
