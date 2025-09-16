using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace NitroTechWebsite
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            var session = (HttpSessionState)HttpContext.Current?.Session;
            if (session != null)
            {
                Account.LogUserLogoff(new HttpSessionStateWrapper(session));
            }
        }
    }
}