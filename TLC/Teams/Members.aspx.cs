using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.Teams
{
    public partial class Members : System.Web.UI.Page
    {
        protected int teamId;
        protected TeamMemberRepository tmmbRepo = new TeamMemberRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            hdfShowModal.Value = "0";
            if (Request.QueryString.GetValues("Id").Length != 0)
            {
                teamId = Convert.ToInt32(Request.QueryString["Id"].ToString());
            }
            if (!Page.IsPostBack)
            {

                LoadMembers(teamId);
            }
        }

        void LoadMembers(object datasource = null)
        {
            if (!Equals(datasource, null))
            {
                grdMembers.DataSource = datasource;
            }
            grdMembers.DataBind();
        }
        void LoadMembers(int teamid)
        {
            var myTeam = new TeamRepository().FindBy(teamid);
            ltrHeader.Text = myTeam.Name + " Members";
            LoadMembers(myTeam.Members);
        }

        void LoadAddMembersList()
        {
            lstMembers.DataSource = (from members in tmmbRepo.GetAll()
                                    where members.TeamId != teamId
                                    orderby members.FullName
                                    select members).ToList();
            lstMembers.DataBind();
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            hdfShowModal.Value = "1";
            txtnewMemberEmail.Value = "";
            txtnewMemberPhone.Value = "";
            txtnewMemberName.Text = "";
            LoadAddMembersList();
        }

        protected void grdMembers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int teamMemberId;
            switch (e.CommandName)
            {      
                case DataControlCommands.DeleteCommandName:
                    teamMemberId = Convert.ToInt32(((GridView)sender).DataKeys[rowIndex][0]);
                    RemoveTeamMember(teamMemberId);
                    LoadMembers(teamId);
                    break;                
                default:
                    break;
            }
            e.Handled = true;
        }
        void RemoveTeamMember(int teamMemberId)
        {

            var teamMember = tmmbRepo.FindBy(teamMemberId);
            teamMember.TeamId = -1;
            tmmbRepo.Update(teamMember);
            tmmbRepo.Save();
        }

        protected void lnkAddMember_Click(object sender, EventArgs e)
        {
            foreach(var item in lstMembers.Items)
            {
                if (((CheckBox)item.FindControl("chkAdd")).Checked)
                {
                    var teamMember = tmmbRepo.FindBy(lstMembers.DataKeys[item.DataItemIndex].Value);
                    teamMember.TeamId = teamId;
                    tmmbRepo.Update(teamMember);
                }
            }
            if (!string.IsNullOrEmpty(txtnewMemberName.Text)){

                TeamMember newMember = new TeamMember();
                List<string> name = txtnewMemberName.Text.Split(' ').ToList();
                newMember.FirstName = name.First();
                name.RemoveAt(0);
                newMember.LastName = string.Join(" ", name.ToArray());
                newMember.Phone = txtnewMemberPhone.Value;
                newMember.Email = txtnewMemberEmail.Value;
                newMember.TeamId = teamId;
                tmmbRepo.Add(newMember);
            }
            tmmbRepo.Save();
            LoadMembers(teamId);
        }
    }
}