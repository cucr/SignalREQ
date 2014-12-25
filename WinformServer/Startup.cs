using Owin;
using Microsoft.Owin.Cors;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WinformServer
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            //IE9下可用
            HubConfiguration hc = new HubConfiguration();
            hc.EnableJSONP = true;
            app.MapSignalR(hc);
        }
    }


}
