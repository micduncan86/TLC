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
            hdfShowModal.Value = "0";
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
                if (!String.IsNullOrEmpty(Request.Params.Get("AddCheckup")))
                {
                    btnAddCheckUp_Click(null, null);
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
                grdCheckUps.Columns[1].Visible = true;
                btnAddCheckUp.Style.Add("display", "none");
            }
            else
            {
                grdCheckUps.Columns[1].Visible = false;
                btnAddCheckUp.Style.Remove("display");
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
        protected void LoadMethods(int methodId = -1)
        {
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

        protected void LoadModalData(CheckUp data = null)
        {
            hdfShowModal.Value = "1";
            LoadMethods();
            if (data == null)
            {
                ltrModalTitle.Text = "Add Check Up for " + ddlMembers.SelectedItem.Text;
                txtTeamId.Value = ddlTeams.SelectedValue;
                txtMemberId.Value = ddlMembers.SelectedValue;
                txtOutcome.Text = "";
                txtActions.Text = "";
                chkActionRequired.Checked = false;
                ddlMethod.SelectedIndex = -1;
                txtCheckUpDate.Text = "";
                lnkAddCheckUp.CommandName = "New";
                lnkAddCheckUp.Text = "<span class='glyphicon glyphicon-plus'></span> Add Check Up";
            }
            else
            {
                ltrModalTitle.Text = "Add Check Up for " + data.Member.FullName;
                txtTeamId.Value = data.TeamId.ToString();
                txtMemberId.Value = data.TeamMemberId.ToString();
                txtOutcome.Text = data.Outcome;
                txtActions.Text = data.Actions;
                chkActionRequired.Checked = data.RequiresAction;
                ddlMethod.SelectedValue = ddlMethod.Items.FindByText(data.Method).Value;
                txtCheckUpDate.Text = data.CheckUpDate.ToShortDateString();
                lnkAddCheckUp.CommandName = "Update";
                lnkAddCheckUp.CommandArgument = data.CheckUpId.ToString();
                lnkAddCheckUp.Text = "<span class='glyphicon glyphicon-check'></span> Update Check Up";
            }
            
        }

        protected void btnAddCheckUp_Click(object sender, EventArgs e)
        {
            LoadModalData(null);
        }

        protected void lnkAddCheckUp_Click(object sender, EventArgs e)
        {
            DateTime checkupDate = DateTime.MinValue;
            DateTime.TryParse(txtCheckUpDate.Text, out checkupDate);
            var btn = sender as LinkButton;
            if (!Equals(checkupDate, DateTime.MinValue))
            { 
                var nCheckUp = btn.CommandName == "New" ? new CheckUp() : checkUpManager.FindBy(Convert.ToInt32(btn.CommandArgument));
                if (nCheckUp.CheckUpId == 0)
                {
                    nCheckUp.TeamId = Convert.ToInt32(txtTeamId.Value);
                    nCheckUp.TeamMemberId = Convert.ToInt32(txtMemberId.Value);
                }                
                nCheckUp.Method = ddlMethod.SelectedItem.Text;
                nCheckUp.CheckUpDate = checkupDate;
                nCheckUp.Outcome = txtOutcome.Text;
                nCheckUp.RequiresAction = chkActionRequired.Checked;
                nCheckUp.Actions = txtActions.Text;
                if (nCheckUp.CheckUpId == 0)
                {
                    checkUpManager.Add(nCheckUp);
                    ((SiteMaster)Page.Master).AddNotification(Page, "Check Up Added Successful", "Check Up for " + ddlMembers.SelectedItem.Text + " was added.");
                }
                else {
                    checkUpManager.Update(nCheckUp);
                    ((SiteMaster)Page.Master).AddNotification(Page, "Check Up Updated Successful", "Check Up for " + ddlMembers.SelectedItem.Text + " was updated.");
                }
                checkUpManager.Save();
                
                LoadCheckUps(null, Convert.ToInt32(ddlTeams.SelectedValue), Convert.ToInt32(ddlMembers.SelectedValue));
            }
        }

        protected void grdCheckUps_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case DataControlCommands.EditCommandName:
                    var checkup = checkUpManager.FindBy(((GridView)sender).DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
                    LoadModalData(checkup);
                    break;

            }
            e.Handled = true;
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