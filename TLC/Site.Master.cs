using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using TLC.Data;

namespace TLC
{
    public partial class SiteMaster : MasterPage
    {
        protected string GetApplicationVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;


        public User loggedUser = null;

        public void AddNotification(Page _page, string Title, string Message, string callback = "")
        {
            ScriptManager.RegisterStartupScript(_page, _page.GetType(), _page.UniqueID + "_Notification", "$(document).ready(function(){" + string.Format("app.SuccessAlert('{0}','{1}',{2});", Title, Message, callback == "" ? "null" : callback) + "});", true);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            //var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            //Guid requestCookieGuidValue;
            //if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            //{
            //    // Use the Anti-XSRF token from the cookie
            //    _antiXsrfTokenValue = requestCookie.Value;
            //    Page.ViewStateUserKey = _antiXsrfTokenValue;
            //}
            //else
            //{
            //    // Generate a new Anti-XSRF token and save to the cookie
            //    _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
            //    Page.ViewStateUserKey = _antiXsrfTokenValue;

            //    var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            //    {
            //        HttpOnly = true,
            //        Value = _antiXsrfTokenValue
            //    };
            //    if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            //    {
            //        responseCookie.Secure = true;
            //    }
            //    Response.Cookies.Set(responseCookie);
            //}

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    // Set Anti-XSRF token
            //    ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            //    ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            //}
            //else
            //{
            //    // Validate the Anti-XSRF token
            //    if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
            //        || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            //    {
            //        throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
            //    }
            //}
            if (Equals(Session["mylogin"], null))
            {
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                Response.Redirect(System.Web.Security.FormsAuthentication.LoginUrl);
                return;
            }

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.User.Identity is FormsIdentity)
                {
                    CacheManager cache = new CacheManager();
                    FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket tkt = id.Ticket;
                    if (cache.CacheList.ContainsKey("LoggedInUser"))
                    {
                        loggedUser = cache.CacheList["LoggedInUser"] as TLC.Data.User;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(tkt.UserData) && !string.IsNullOrWhiteSpace(tkt.UserData))
                        {
                            var jSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            var json = System.Text.Encoding.Default.GetString(Convert.FromBase64String(tkt.UserData));
                            var lgn = jSerializer.Deserialize(json, new User().GetType());
                            loggedUser = (TLC.Data.User)lgn;
                        }
                    }
                    HttpContext.Current.User = new GenericPrincipal(id, loggedUser.Role.Split(','));
                }
            }
        }
        

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            CacheManager cache = new CacheManager();
            cache.CacheList.Clear();
            cache.Clear();            
        }
    }

}