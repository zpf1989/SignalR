using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ServiceMonitor.Startup))]

namespace ServiceMonitor
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR("/signalr/monitor", new Microsoft.AspNet.SignalR.HubConfiguration { EnableDetailedErrors = false, EnableJavaScriptProxies = true });
            //app.MapSignalR();
        }
    }
}
