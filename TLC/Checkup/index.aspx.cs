using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.Checkup
{
    public partial class index : System.Web.UI.Page
    {
        protected CheckUpRepository checkUpManager = new CheckUpRepository();
        protected TeamRepository teamManager = new TeamRepository();
        protected MemberRepository memberManager = new MemberRepository();
        int memberId = 0, teamId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.Params.Get("MemberId"),out memberId);
            int.TryParse(Request.Params.Get("TeamId"), out teamId);
            if (!Page.IsPostBack)
            {
                teamId = teamId == 0 ? memberManager.FindBy(memberId).TeamId : teamId;
                LoadTeams(teamId);
                LoadMembers(memberId);
                LoadCheckUps(null,teamId,memberId);
                if (User.IsInRole(UserRepository.ReturnUserRole(Data.User.enumRole.Administrater)))
                {
                    lblTeams.Visible = true;
                    ddlTeams.Visible = true;
                }
            }
        }

        protected void LoadCheckUps(object datasource = null,int teamId =0, int memberId = 0)
        {
            if (datasource == null)
            {
                if (teamId != 0 && memberId == 0)
                {
                    datasource = checkUpManager.GetCheckUpsByTeamId(teamId);
                }
                if (memberId != 0)
                {
                    datasource = checkUpManager.GetCheckUpsByMemberId(memberId);
                }
                if (datasource == null)
                {
                    datasource = new List<CheckUp>();
                }
            }
            if (ddlMembers.SelectedValue == "0")
            {
                grdCheckUps.Columns[0].Visible = true;
            }
            else
            {
                grdCheckUps.Columns[0].Visible = false;
            }
            grdCheckUps.DataSource = datasource;
            grdCheckUps.DataBind();
        }
        protected void LoadTeams(int teamId = 0)
        {
            var teams = teamId == 0 ? teamManager.GetAll() : new List<Team>() { teamManager.FindBy(teamId) };

            if (User.IsInRole(UserRepository.ReturnUserRole(Data.User.enumRole.Administrater)))
            {
                teams = teamManager.GetAll();
            }

            ddlTeams.DataSource = teams;
            ddlTeams.DataTextField = "TeamName";
            ddlTeams.DataValueField = "TeamId";
            ddlTeams.DataBind();
        }

        protected void ddlTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            teamId = Convert.ToInt32(ddlTeams.SelectedValue);
            LoadMembers();
            btnLoadCheckUps_Click(null, null);
        }

        protected void btnLoadCheckUps_Click(object sender, EventArgs e)
        {
            LoadCheckUps(null, Convert.ToInt32(ddlTeams.SelectedValue), Convert.ToInt32(ddlMembers.SelectedValue));
        }

        protected void LoadMembers(int memberId = 0)
        {
            var members = memberManager.GetMembersByTeamId(teamId);          

            ddlMembers.DataSource = members;
            ddlMembers.DataTextField = "FullName";
            ddlMembers.DataValueField = "MemberId";
            ddlMembers.DataBind();

            ddlMembers.Items.Add(new ListItem("All", "0"));
            if (ddlMembers.Items.Cast<ListItem>().Where(x => x.Value == memberId.ToString()).Count() > 0){
                ddlMembers.SelectedValue = memberId.ToString();
            }            
        }
    }
}