using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;
namespace TLC.Users
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected UserRepository UserManager = new UserRepository();
        private int UserId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserId = ((SiteMaster)Page.Master).loggedUser.UserId;
            if (Request.Params.Get("UserId") != null)
            {                
                UserId = Convert.ToInt32(Request.Params.Get("UserId"));
            }
            if (!Page.IsPostBack)
            {
                var user = UserManager.FindBy(UserId);
                txtEmail.Text = user.Email;

                if (User.IsInRole(UserRepository.ReturnUserRole(Data.User.enumRole.Administrater)))
                {
                    txtOldPassword.Visible = false;
                }
            }
        }

        protected void lnkChange_Click(object sender, EventArgs e)
        {
            int status = 0;
            divOutcome.Visible = false;
            lblError.Visible = false;
            try
            {
                if (Request.Params.Get("UserId") != null)
                {
                    status = UserManager.ChangePassword(UserId, txtEmail.Text, txtPassword.Text);
                }
                else
                {
                    status = UserManager.ChangePassword(UserId, txtEmail.Text, txtOldPassword.Text, txtPassword.Text);
                }
                    
                if (status == 1)
                {
                    divOutcome.Visible = true;
                    divForm.Visible = false;
                    return;
                }
                ShowError("Could not find record. Matching email and original password do not match. Try again.");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                //throw;
            }
        }
        protected void ShowError(string ErroMsg)
        {
            divForm.Visible = true;
            lblError.Visible = true;
            lblError.Text = "Something went wrong with changing your password.";
            lblError.Text += "<p>" + ErroMsg + "</p>";
        }
    }
}