using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Optimization;
using System.Security.Principal;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using TLC.Data;

namespace TLC
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = System.Web.Http.RouteParameter.Optional });
        }


        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (User != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (User.Identity is FormsIdentity)
                    {
                        FormsIdentity id = (FormsIdentity)User.Identity;
                        FormsAuthenticationTicket tkt = id.Ticket;
                        var roles = tkt.UserData.Split(':')[1];
                        HttpContext.Current.User = new GenericPrincipal(id, roles.Split(','));                        
                    }
                }
            }
           
        }

    }
}