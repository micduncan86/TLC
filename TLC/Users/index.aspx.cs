using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.Users
{
    public partial class index : System.Web.UI.Page
    {
        protected UserRepository UserManager = new UserRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            hdfShowModal.Value = "0";
            if (!Page.IsPostBack)
            {
                LoadUsers();
            }
        }
        protected void LoadUsers(object userDataSource = null)
        {
            if (userDataSource == null)
            {
                userDataSource = UserManager.GetAll();
            }
            lstUsers.DataSource = userDataSource;
            lstUsers.DataBind();
        }
        protected void FillTeamDropDown(int teamid = -1)
        {
            if (teamid == -1)
            {
                ddlTeam.DataSource = new TeamRepository().GetAll().Where(x => !UserManager.GetAll().Select(y => y.MyTeamId).Contains(x.TeamId)).ToList();
            }
            else
            {
                ddlTeam.DataSource = new TeamRepository().GetAll();
            }

            ddlTeam.DataTextField = "TeamName";
            ddlTeam.DataValueField = "TeamId";
            ddlTeam.DataBind();
            if (teamid != -1)
            {
                ddlTeam.SelectedValue = teamid.ToString();
            }
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            hdfShowModal.Value = "1";
            hdnUserId.Value = "-1";
            FillTeamDropDown(-1);

            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtPassword.Visible = true;
            txtUserName.Text = string.Empty;

            reqEmail.Enabled = true;
            reqPassword.Enabled = true;
        }

        protected void lnkAddUser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                User newUser = UserManager.AddUser(txtEmail.Text, txtPassword.Text);
                newUser.UserName = string.IsNullOrWhiteSpace(txtUserName.Text) ? txtEmail.Text : txtUserName.Text;
                newUser.UserRole = chkIsAdmin.Checked ? Data.User.enumRole.Administrater : Data.User.enumRole.User;

                newUser.MyTeamId = chkIsAdmin.Checked ? -1 : Convert.ToInt32(ddlTeam.SelectedValue);

                UserManager.Update(newUser);
                UserManager.Save();
                if (newUser.MyTeamId > 0)
                {
                    var provider = new TeamRepository();
                    var myteam = provider.FindBy(newUser.MyTeamId);
                    if (myteam != null)
                    {
                        myteam.TeamLeaderId = newUser.UserId;
                        provider.Update(myteam);
                        provider.Save();
                    }
                }
            }
            LoadUsers();
            SiteMaster master = (SiteMaster)Page.Master;
            master.AddNotification(Page, "Add Successful", "A new user has been added to the system.");
        }
        protected void EditUser(User data)
        {
            hdfShowModal.Value = "1";
            hdnUserId.Value = data.UserId.ToString();
            txtEmail.Text = data.Email;
            txtUserName.Text = data.UserName;
            FillTeamDropDown(data.MyTeamId);
            reqEmail.Enabled = true;
            reqPassword.Enabled = false;
            txtPassword.Visible = false;
            chkIsAdmin.Checked = data.UserRole == Data.User.enumRole.Administrater;
        }
        protected void lnk_Command(object sender, CommandEventArgs e)
        {
            var UserId = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "EditUser":
                    EditUser(UserManager.FindBy(UserId));
                    break;
                case "ChangePassword":
                    break;
                case DataControlCommands.DeleteCommandName:
                    UserManager.Delete(UserId);
                    UserManager.Save();
                    LoadUsers();
                    SiteMaster master = (SiteMaster)Page.Master;
                    master.AddNotification(Page, "Delete Complete", "You have delete a user.");
                    break;
            }
        }

        protected void lstUsers_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var user = e.Item.DataItem as TLC.Data.User;
                if (((SiteMaster)Page.Master).loggedUser != null && ((SiteMaster)Page.Master).loggedUser.UserId == user.UserId)
                {
                    e.Item.FindControl("lnkRemove").Visible = false;
                }                
            }

        }
    }
}