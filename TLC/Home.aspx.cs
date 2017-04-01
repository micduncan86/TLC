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
                    LoadTeams(String.IsNullOrEmpty(HttpUtility.ParseQueryString(Request.Url.Query).Get("TeamId")) ? mylogin.MyTeamId : Convert.ToInt32(HttpUtility.ParseQueryString(Request.Url.Query).Get("TeamId")));
                }
                else
                {
                    LoadTeams(mylogin.MyTeamId);
                }
                            
                LoadTeam(null);
                lstTeams.Visible = mylogin.UserRole == Data.User.enumRole.Administrater ? true : false;    
            }
        }
        protected void LoadTeams(int teamid)
        {
            var data = new TeamRepository().GetAll();
            lstTeams.DataSource = data;
            lstTeams.DataBind();
            if (teamid == -1)
            {
                if (lstTeams.DataKeys.Count > 0)
                {
                    teamid = Convert.ToInt32(lstTeams.DataKeys[0].Value);
                }                
            }
            hdnTeamId.Value = teamid.ToString();
        }
        protected void LoadTeam(Team data)
        {
            if (data == null)
            {
                data = new TeamRepository().FindBy(Convert.ToInt32(hdnTeamId.Value));
            }
            if (data != null)
            {
                hdnTeamId.Value = data.TeamId.ToString();
                txtTeamName.Text = data.TeamName;
                ltrMemberCount.Text = data.Members.Count.ToString();

                txtTeamNumber.Text = data.TeamNumber;
                   lblTeamLeader.Text = data.TeamLeader != null ? data.TeamLeader.UserName : "Not Assigned";
                lblCoLeader.Text = data.CoTeamLeader.UserName;
                var Events = (List<TLC.Data.Event>)data.Events;

                var Members = (List<TLC.Data.Member>)data.Members;

                LoadEvents(Events);
                LoadMembers(Members);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Page.UniqueID + "_ModalNotification", string.Format("app.modalFunction('{0}','{1}',{2},{3});", "Could Not Load Team Data.", "You are no longer a leader of a team. Please contact your administrator.", "null", "function(){ window.location='../account/login.aspx';}"), true);
                Session["mylogin"] = null;
            }
            
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

 
        protected void lstMembers_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            var lst = sender as ListView;
            switch (e.CommandName)
            {
                case DataControlCommands.DeleteCommandName:
                    int memberId = Convert.ToInt32(lst.DataKeys[e.Item.DataItemIndex].Value);
                    AddRemoveMember(memberId, 0);
                    LoadTeam(null);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SUCCESSMSG", "app.SuccessAlert('Success','Members have been removed from the team.');", true);
                    break;  
            }
            e.Handled = true;
        }

        protected void lnkUpdateTeamInfo_Click(object sender, EventArgs e)
        {
            var provider = new TeamRepository();
            Team updateTeam = provider.FindBy(Convert.ToInt32(hdnTeamId.Value));
            updateTeam.TeamName = txtTeamName.Text;
            updateTeam.TeamNumber = txtTeamNumber.Text;

            provider.Update(updateTeam);
            provider.Save();
            ((SiteMaster)Page.Master).AddNotification(Page, "Update Successful", "Team information has been updated.");

        }
    }
}