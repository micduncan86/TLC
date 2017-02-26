using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Data.SqlClient;
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
            AlterDatabase();
        }
        

        protected void AlterDatabase()
        {
            List<SqlCommand> dbChanges = new List<SqlCommand>();
            var sqlCommands = File.ReadAllText(HttpContext.Current.Server.MapPath(@"dbChanges/sql.txt")).Split(';');
            foreach(string sql in sqlCommands)
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    dbChanges.Add(new SqlCommand(sql));
                }
                
            }

            if (dbChanges.Count > 0)
            {
                try
                {
                    foreach(var cmd in dbChanges){
                        cmd.Connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DataDb"].ToString());
                        using (cmd)
                        {
                            cmd.Connection.Open();
                            cmd.ExecuteNonQuery();
                            cmd.Connection.Close();
                        }
                    }
                }catch (Exception e)
                {
                    throw;
                }
            }
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