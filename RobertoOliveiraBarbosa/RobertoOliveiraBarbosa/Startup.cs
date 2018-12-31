using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RobertoOliveiraBarbosa.Startup))]
namespace RobertoOliveiraBarbosa
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
