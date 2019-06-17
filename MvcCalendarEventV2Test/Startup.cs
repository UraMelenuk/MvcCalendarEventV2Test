using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcCalendarEventV2Test.Startup))]
namespace MvcCalendarEventV2Test
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
