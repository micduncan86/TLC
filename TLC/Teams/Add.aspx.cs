using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TLC.Teams
{
    public partial class Add : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadLeaders();
                if (Request.QueryString.AllKeys.ToList().Select(x => x.Contains("id")).SingleOrDefault())
                {
                    LoadTeamData(Convert.ToInt32(Request.QueryString["id"]));
                }
            }
        }
        protected void LoadLeaders()
        {
            TeamLeaderId.DataSource = new TLC.Data.TeamMemberRepository().GetAll();
            TeamLeaderId.DataTextField = "FullName";
            TeamLeaderId.DataValueField = "TeamMemberId";
            TeamLeaderId.DataBind();
        }
        protected void LoadTeamData(int teamId)
        {
            var TeamRepo = new TLC.Data.TeamRepository();
            var team = TeamRepo.FindBy(teamId);
            TeamName.Value = team.Name;
            GroupNumber.Value = team.GroupNumber;
            TeamLeaderId.SelectedValue = team.TeamLeaderId.ToString();            
        }
    }
}