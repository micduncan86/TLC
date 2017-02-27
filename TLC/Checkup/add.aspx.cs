using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.Checkup
{
    public partial class add : System.Web.UI.Page
    {
        TeamRepository teamManager = new TeamRepository();
        MemberRepository memberManager = new MemberRepository();
        int memberId = -1;
        int teamId = -1;

        //public struct CheckUpMethod
        //{
        //    public string name;
        //    public int id;
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.Params.Get("MemberId"), out memberId);
            int.TryParse(Request.Params.Get("TeamId"), out teamId);
            if (!Page.IsPostBack)
            {                
                LoadMembers(memberId);
                LoadTeams(teamId);
                LoadMethods();
            }
        }

        protected void LoadTeams(int teamId = 0)
        {
            var teams = teamId == 0 ? teamManager.GetAll() : new List<Team>() { teamManager.FindBy(teamId) };

            ddlTeam.DataSource = teams;
            ddlTeam.DataTextField = "TeamName";
            ddlTeam.DataValueField = "TeamId";
            ddlTeam.DataBind();
        }
        protected void LoadMembers(int memberId = 0)
        {
            var teams = memberId == 0 ? memberManager.GetAll() : new List<Member>() { memberManager.FindBy(memberId) };

            if (teams.Count() == 1 && teamId == 0)
            {
                teamId = teams.First().TeamId;
            }

            ddlMember.DataSource = teams;
            ddlMember.DataTextField = "FullName";
            ddlMember.DataValueField = "MemberId";
            ddlMember.DataBind();
        }
        protected void LoadMethods(int methodId = -1)
        {
            //var teams = new List<CheckUpMethod>();
            //teams.Add(new CheckUpMethod() { name="Phone Call", id=1 });
            //teams.Add(new CheckUpMethod() { name = "Email", id = 2 });
            //teams.Add(new CheckUpMethod() { name = "In Person", id = 3 });
            //teams.Add(new CheckUpMethod() { name = "Mail", id = 4 });

            var methods = new List<object>();            
            methods.Add(new { name = "Phone Call", id = 1 });
            methods.Add(new { name = "Email", id = 2 });
            methods.Add(new { name = "In Person", id = 3 });
            methods.Add(new { name = "Mail", id = 4 });            

            ddlMethod.DataSource = methods;
            ddlMethod.DataTextField = "name";
            ddlMethod.DataValueField = "id";
            ddlMethod.DataBind();
        }

        protected void lnkAddCheckUp_Click(object sender, EventArgs e)
        {
            DateTime checkupDate = DateTime.MinValue;
            DateTime.TryParse(txtCheckUpDate.Text, out checkupDate);
            if (!Equals(checkupDate, DateTime.MinValue)){
                CheckUpRepository checkUpManager = new CheckUpRepository();
                var nCheckUp = new CheckUp();
                nCheckUp.TeamId = Convert.ToInt32(ddlTeam.SelectedValue);
                nCheckUp.TeamMemberId = Convert.ToInt32(ddlMember.SelectedValue);
                nCheckUp.Method = ddlMethod.SelectedItem.Text;
                nCheckUp.CheckUpDate = checkupDate;
                nCheckUp.Outcome = txtOutCome.Text;
                nCheckUp.RequiresAction = chkActionRequired.Checked;
                nCheckUp.Actions = txtFollowUpAction.Text;

                checkUpManager.Add(nCheckUp);
                checkUpManager.Save();
                //((SiteMaster)Page.Master).AddNotification(Page, "Check Up Added Successful", "Check Up for " + ddlMember.SelectedItem.Text + " was added.","function(){window.location = '" + Page.ResolveUrl("~/home.aspx?Team=" + nCheckUp.TeamId) + "';}");
                Response.Redirect("~/home.aspx?Team=" + nCheckUp.TeamId);
            }
        }
    }
}