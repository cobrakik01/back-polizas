using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Com.PGJ.SistemaPolizas.Startup))]
namespace Com.PGJ.SistemaPolizas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
