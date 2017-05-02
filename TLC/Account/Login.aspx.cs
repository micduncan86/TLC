using System;
using System.Net;
using System.Threading;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using TLC.Data;

namespace TLC.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void SetTicketAuth(User login)
        {
            var jSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            FormsAuthenticationTicket lgnTicket = new FormsAuthenticationTicket(1, login.UserName, DateTime.Now, DateTime.Now.AddSeconds((60 * 15)), true, Convert.ToBase64String(Encoding.Default.GetBytes(jSerializer.Serialize(login))));
            string encryptTicket = FormsAuthentication.Encrypt(lgnTicket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptTicket);
            cookie.Expires = lgnTicket.Expiration;
            Response.Cookies.Add(cookie);      

        }
        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                var provider = new UserRepository();
                //provider.AddUser(Email.Text, Password.Text);
                var login = provider.Authenticate(Email.Text, Password.Text);
                if (login != null)
                {
                    Session.Add("mylogin", login);
                    SetTicketAuth(login);
                    var cache = new CacheManager();
                    if (!cache.CacheList.ContainsKey("LoggedInUser"))
                    {                        
                        cache.CacheList.Add("LoggedInUser", login);
                    }
                    if (Request.Url.Query.Contains("ReturnUrl"))
                    {
                        Response.Redirect(HttpUtility.ParseQueryString(Request.Url.Query).Get("ReturnUrl"));
                    }
                    else
                    {
                        Response.Redirect("~/Home.aspx");
                    }
                }
                ErrorMessage.Visible = true;
                FailureText.Text = "Combination of Username and Password did not match. Please try again.";



            }
        }
    }
}