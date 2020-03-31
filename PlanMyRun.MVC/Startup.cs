
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PlanMyRun.MVC.Startup))]
namespace PlanMyRun.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
