using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.Reports
{
    public partial class parameters : System.Web.UI.Page
    {
        protected SiteMaster _master;
        protected CacheManager cache;
        protected void Page_Load(object sender, EventArgs e)
        {
            cache = new CacheManager();
            _master = Page.Master as SiteMaster;
            if (!Page.IsPostBack)
            {
                FillReports(ddlReports);
                if (cache.CacheList.ContainsKey("rptParams"))
                {
                    var rptParams = cache.CacheList["rptParams"] as ReportParameters;
                    txtFrom.Text = rptParams.FromDate == DateTime.MinValue ? "" : rptParams.FromDate.ToShortDateString();
                    txtTo.Text = rptParams.EndDate == DateTime.MaxValue ? "" : rptParams.EndDate.ToShortDateString();
                    if (ddlReports.Items.FindByValue(rptParams.Report) != null)
                    {
                        ddlReports.SelectedValue = rptParams.Report;
                        ddlReports_SelectedIndexChanged(null, null);
                        if (ddlTeams.Items.FindByValue(rptParams.TeamId.ToString()) != null)
                        {
                            ddlTeams.SelectedValue = rptParams.TeamId.ToString();
                            ddlTeams_SelectedIndexChanged(null, null);
                        }
                        if (ddlMembers.Items.FindByValue(rptParams.MemberId.ToString()) != null)
                        {
                            ddlMembers.SelectedValue = rptParams.MemberId.ToString();
                        }
                    }
                }
            }
        }

        private void FillReports(DropDownList ddl)
        {
            var reports = new ReportRepository().GetAll();
            foreach (Report _rpt in reports)
            {
                ddl.Items.Add(new ListItem(_rpt.Name));
            }
            ddl.Items.Insert(0, new ListItem(""));
        }

        protected void ddlReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            divTeams.Visible = false;
            divMembers.Visible = false;
            divDateRange.Visible = false;
            if (ddlReports.SelectedItem == null || String.IsNullOrWhiteSpace(ddlReports.SelectedItem.Text))
            {
                return;
            }
            var rptType = ReportRepository.ReportType(ddlReports.SelectedItem.Text);
            switch (rptType)
            {
                case ReportRepository.rptNames.None:
                case ReportRepository.rptNames.TeamList:
                    break;
                default:
                    divTeams.Visible = User.IsInRole(UserRepository.ReturnUserRole(Data.User.enumRole.Administrater));
                    FillTeams();
                    break;

            }
            if (rptType == ReportRepository.rptNames.MemberFollowUp)
            {
                
                int TeamId = 0;
                int.TryParse(ddlTeams.SelectedValue, out TeamId);
                var myTeam = new TeamRepository().FindBy(TeamId);
                FillMembers(myTeam);                
            }
            if (rptType == ReportRepository.rptNames.MemberFollowUp || rptType == ReportRepository.rptNames.TeamCheckUps || rptType == ReportRepository.rptNames.TeamEvents)
            {
                divDateRange.Visible = true;
            }
        }
        protected void FillTeams()
        {
            ddlTeams.Items.Clear();
            var providier = new TeamRepository();
            var datasource = providier.GetAll();
            if (!User.IsInRole(UserRepository.ReturnUserRole(Data.User.enumRole.Administrater)))
            {

                datasource = datasource.Where(t => t.TeamId == _master.loggedUser.MyTeamId).ToList();
            }
            ddlTeams.DataSource = datasource;
            ddlTeams.DataTextField = "TeamName";
            ddlTeams.DataValueField = "TeamId";
            ddlTeams.DataBind();
            if (User.IsInRole(UserRepository.ReturnUserRole(Data.User.enumRole.Administrater)))
            {
                ddlTeams.Items.Insert(0, new ListItem("All", "-1"));
            }
        }
        protected void FillMembers(Team myTeam)
        {
            ddlMembers.Items.Clear();
            if (myTeam != null)
            {
                ddlMembers.DataSource = myTeam.Members;
                ddlMembers.DataTextField = "FullName";
                ddlMembers.DataValueField = "MemberId";
                ddlMembers.DataBind();

            }
            ddlMembers.Items.Insert(0, new ListItem("All", "-1"));
            divMembers.Visible = ddlMembers.Items.Count > 1;
        }

        protected void ddlTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            int TeamId = 0;
            int.TryParse(ddlTeams.SelectedValue, out TeamId);
            var myTeam = new TeamRepository().FindBy(TeamId);
            FillMembers(myTeam);
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            var rptParams = new ReportParameters();

            rptParams.Report = ddlReports.SelectedValue;
            int TeamdId = -1;
            int MemberId = -1;
            DateTime FromDate = DateTime.MinValue;
            DateTime ToDate = DateTime.MaxValue;

            int.TryParse(ddlTeams.SelectedValue, out TeamdId);
            int.TryParse(ddlMembers.SelectedValue, out MemberId);
            if (!String.IsNullOrWhiteSpace(txtFrom.Text))
            {
                DateTime.TryParse(txtFrom.Text, out FromDate);
            }
            if (!String.IsNullOrWhiteSpace(txtTo.Text))
            {
                DateTime.TryParse(txtTo.Text, out ToDate);
            }              

            rptParams.TeamId = TeamdId;
            rptParams.MemberId = MemberId;
            rptParams.FromDate = FromDate;
            rptParams.EndDate = ToDate;

            if (cache.CacheList.ContainsKey("rptParams"))
            {
                cache.CacheList["rptParams"] = rptParams;
            }
            else
            {
                cache.CacheList.Add("rptParams", rptParams);
            }


            var script = string.Format("ShowReport(\"{0}\");", rptParams.Report);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowReport", script, true);
        }
    }
}