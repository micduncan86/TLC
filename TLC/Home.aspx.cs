using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;
namespace TLC
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Equals(Session["mylogin"], null))
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    return;
                }
                var mylogin = Session["mylogin"] as User;
                if (User.IsInRole(UserRepository.ReturnUserRole(Data.User.enumRole.Administrater)))
                {
                    LoadTeams(String.IsNullOrEmpty(HttpUtility.ParseQueryString(Request.Url.Query).Get("Team")) ? mylogin.MyTeamId : Convert.ToInt32(HttpUtility.ParseQueryString(Request.Url.Query).Get("Team")));
                }
                else
                {
                    LoadTeams(mylogin.MyTeamId);
                }
                            
                LoadTeam(null);
                ddlTeams.Visible = mylogin.MyTeamId == -1 ? true : false;    
            }
        }
        protected void LoadTeams(int teamid)
        {
            var data = new TeamRepository().GetAll();
            ddlTeams.DataSource = data;
            ddlTeams.DataTextField = "TeamName";
            ddlTeams.DataValueField = "TeamId";
            ddlTeams.DataBind();
            ddlTeams.SelectedValue = teamid.ToString();
        }
        protected void LoadTeam(Team data)
        {
            if (data == null)
            {
                data = new TeamRepository().FindBy(Convert.ToInt32(ddlTeams.SelectedValue));
            }
            ltrTeamName.Text = data.TeamName;
            ltrMemberCount.Text = data.Members.Count.ToString();

            lblTeamNumber.Text = data.TeamNumber;
            lblCoLeader.Text = "None";  
            lnkAddMember.CommandArgument = data.TeamId.ToString();
            if (data.CoTeamLeader != null)
            {
                lblCoLeader.Text = String.IsNullOrWhiteSpace(data.CoTeamLeader.FullName) ? "None" : data.CoTeamLeader.FullName;
            }
            LoadEvents(data.Events);
            LoadMembers(data.Members);
        }

        protected void LoadEvents(List<Event> data)
        {
            ltrEventCount.Text = data.Count.ToString();
            lstEvents.DataSource = data;
            lstEvents.DataBind();
        }
        protected void LoadMembers(List<Member> data)
        {
            lstMembers.DataSource = data;
            lstMembers.DataBind();

        }

        protected void ddlTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTeam(null);
        }

        protected void lstMembers_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            var btn = e.Item.FindControl("lnkRemoveMember") as LinkButton;
            btn.OnClientClick += "javascript: return confirm('Are you sure you want to remove " + ((Member)e.Item.DataItem).FullName + "?');";
        }


        protected void AddRemoveMember(int memberId, int teamId)
        {
            var tmManager = new MemberRepository();
            var teamMember = tmManager.FindBy(memberId);
            teamMember.TeamId = teamId;
            tmManager.Update(teamMember);
            tmManager.Save();
            
        }

        protected void lnkAddMember_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/members/addtoteam.aspx?team=" + ddlTeams.SelectedValue);
            // Schttp://localhost:16189/HomeriptManager.RegisterStartupScript(this.Page, this.GetType(), "ADDNEWMEMBER", "ModalAddMember(" + ddlTeams.SelectedValue + ");", true);
        }

        protected void lstMembers_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            var lst = sender as ListView;
            switch (e.CommandName)
            {
                case DataControlCommands.DeleteCommandName:
                    int memberId = Convert.ToInt32(lst.DataKeys[e.Item.DataItemIndex].Value);
                    AddRemoveMember(memberId, -1);
                    LoadTeam(null);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SUCCESSMSG", "app.SuccessAlert('Success','Members have been removed from the team.');", true);
                    break;  
            }
            e.Handled = true;
        }
    }
}