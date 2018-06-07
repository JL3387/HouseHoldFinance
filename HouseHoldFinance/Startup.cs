using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HouseHoldFinance.Startup))]
namespace HouseHoldFinance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
