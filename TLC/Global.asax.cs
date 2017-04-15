using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
                    throw e;
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            HttpUnhandledException httpUnhandledException = new HttpUnhandledException(Server.GetLastError().Message, Server.GetLastError());

            List<string> errorSendTo = new List<string>();
            errorSendTo.Add("mbd2682@email.vccs.edu");
            TLC.Data.Email.Send(errorSendTo, "Application Exception Raised" , httpUnhandledException.GetHtmlErrorMessage());
    
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (User != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (User.Identity is FormsIdentity)
                    {
                        CacheManager cache = new CacheManager();
                        FormsIdentity id = (FormsIdentity)User.Identity;
                        FormsAuthenticationTicket tkt = id.Ticket;
                        TLC.Data.User lgn = null;
                        if (!cache.CacheList.ContainsKey("LoggedInUser"))
                        {
                            var jSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var json = System.Text.Encoding.Default.GetString(Convert.FromBase64String(tkt.UserData));
                            lgn = jSerializer.Deserialize<TLC.Data.User>(json);
                            cache.CacheList.Add("LoggedInUser", lgn);
                        }
                        else
                        {
                            lgn = cache.CacheList["LoggedInUser"] as TLC.Data.User;
                        }             
                        HttpContext.Current.User = new GenericPrincipal(id, lgn.Role.Split(','));                        
                    }
                }
            }
           
        }

    }
}