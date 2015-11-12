using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SpecialBreakFast.Startup))]
namespace SpecialBreakFast
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
