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
        protected SiteMaster master = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            master = (SiteMaster)Page.Master;
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

            ddlTeam.DataSource = new TeamRepository().GetAll();

            ddlTeam.DataTextField = "TeamName";
            ddlTeam.DataValueField = "TeamId";
            ddlTeam.DataBind();

            ddlTeam.Items.Insert(0, new ListItem("", "-1"));

            if (ddlTeam.Items.FindByValue(teamid.ToString()) != null)
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
            lblError.Visible = false;
            txtEmail.CssClass = "form-control";

            lnkAddUser.CommandName = DataControlCommands.InsertCommandName;
            lnkAddUser.CommandArgument = "-1";             
        }

        protected void lnkAddUser_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            if (Page.IsValid)
            {
                User myUser = new Data.User();
                switch (btn.CommandName)
                {
                    case DataControlCommands.EditCommandName:
                        myUser = UserManager.FindBy(Convert.ToInt32(btn.CommandArgument));
                        master.AddNotification(Page, "Update Successful", "User was updated.");
                        break;
                    case DataControlCommands.InsertCommandName:
                        myUser = UserManager.AddUser(txtEmail.Text, txtPassword.Text);
                        master.AddNotification(Page, "Add Successful", "A new user has been added to the system.");
                        break;
                }
                if (myUser.UserId != -1)
                {
                    lblError.Visible = false;
                    myUser.UserName = string.IsNullOrWhiteSpace(txtUserName.Text) ? txtEmail.Text : txtUserName.Text;
                    myUser.UserRole = chkIsAdmin.Checked ? Data.User.enumRole.Administrater : Data.User.enumRole.User;

                                         

                    myUser.MyTeamId = Convert.ToInt32(ddlTeam.SelectedValue);
                    var provider = new TeamRepository();
                    var myteam = provider.FindBy(myUser.MyTeamId);
                    if (myteam != null)
                    {
                        //var userProvider = new UserRepository();
                        //var oldLeader = userProvider.FindBy(myteam.TeamLeaderId);
                        //oldLeader.MyTeamId = -1;
                        //userProvider.Update(oldLeader);
                        //userProvider.Save();

                        myteam.TeamLeaderId = myUser.UserId;
                        provider.Update(myteam);
                        provider.Save();
                    }

                    UserManager.Update(myUser);
                    UserManager.Save();                    
                    LoadUsers();
                    
                }
                else
                {
                    hdfShowModal.Value = "1";
                    lblError.Text = "Cannot add user. This email is already registered. Try again.";
                    txtEmail.CssClass += " alert-danger";
                    lblError.Visible = true;
                }

            }

        }
        protected void EditUser(User data)
        {
            hdfShowModal.Value = "1";
            hdnUserId.Value = data.UserId.ToString();
            txtEmail.Text = data.Email;
            txtUserName.Text = data.UserName;
            FillTeamDropDown(data.MyTeamId);
               txtPassword.Visible = false;
            lblError.Visible = false;
            txtEmail.CssClass = "form-control";
            lnkAddUser.Text = "Update User";
            lnkAddUser.CommandName = DataControlCommands.EditCommandName;
            lnkAddUser.CommandArgument = data.UserId.ToString();
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