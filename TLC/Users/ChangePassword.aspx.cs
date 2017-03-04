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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {        
            }
        }

        protected void lnkChange_Click(object sender, EventArgs e)
        {
            int status = 0;  
            divOutcome.Visible = false;
            lblError.Visible = false;
            try
            {
                status = UserManager.ChangePassword(((SiteMaster)Page.Master).loggedUser.UserId, txtEmail.Text, txtOldPassword.Text, txtPassword.Text); 
                if ( status == 1)
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