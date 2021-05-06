using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(r710_fan_control.Startup))]
namespace r710_fan_control
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}