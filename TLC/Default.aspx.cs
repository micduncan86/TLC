using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TLC
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //LoadTeams();
            }
        }

        protected void LoadTeams()
        {
            //lstTeams.DataSource = new TeamController().Get();
            //lstTeams.DataBind();
        }

        protected void lnkRemoveTeam_Click(object sender, EventArgs e)
        {
            int teamId = Convert.ToInt32(((LinkButton)sender).CommandArgument);

            new TeamController().Delete(teamId);
            LoadTeams();
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            LoadTeams();
        }

        protected void lstTeams_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {                
                var members = ((TLC.Data.Team)e.Item.DataItem).Members;
                if (members.Count > 0)
                {
                    var rptmember = e.Item.FindControl("rptMembers") as Repeater;
                    rptmember.Visible = true;
                    rptmember.DataSource = members;
                    rptmember.DataBind();
                }
            }
        }
    }
}