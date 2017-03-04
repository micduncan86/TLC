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
                if (provider != null)
                {
                    //provider.AddUser(Email.Text, Password.Text);
                    var login = provider.Authenticate(Email.Text, Password.Text);
                    if (login != null)
                    {
                        Session.Add("mylogin", login);
                        SetTicketAuth(login);
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

                #region CommentSit
                //Validate the user password
                //var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                //var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                //This doen't count login failures towards account lockout
                // To enable password failures to trigger lockout, change to shouldLockout: true
                //var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

                //switch (result)
                //{
                //    case SignInStatus.Success:
                //        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                //        break;
                //    case SignInStatus.LockedOut:
                //        Response.Redirect("/Account/Lockout");
                //        break;
                //    case SignInStatus.RequiresVerification:
                //        Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                //                                        Request.QueryString["ReturnUrl"],
                //                                        RememberMe.Checked),
                //                          true);
                //        break;
                //    case SignInStatus.Failure:
                //    default:
                //        FailureText.Text = "Invalid login attempt";
                //        ErrorMessage.Visible = true;
                //        break;
                //}
                #endregion
            }
        }
    }
}