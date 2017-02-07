using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.Teams
{
    public partial class Index : System.Web.UI.Page
    {
        protected TeamRepository tmRepo = new TeamRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadGrid();
            }
        }

        void LoadGrid(object datasource = null)
        {
            if (Equals(datasource, null))
            {
                grdTeams.DataSource = tmRepo.GetAll();
            }
            else
            {
                grdTeams.DataSource = datasource;
            }
            
            grdTeams.DataBind();
        }

        void LoadMemberGrid(object datasource = null)
        {
            if (Equals(datasource, null))
            {
                lstMembers.DataSource = tmRepo.GetAll();
            }
            else
            {
                lstMembers.DataSource = datasource;
            }

            lstMembers.DataBind();
        }

        protected void grdTeams_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int teamId;
            switch (e.CommandName)
            {
                case DataControlCommands.EditCommandName:
                             grdTeams.EditIndex = rowIndex;
                    LoadGrid();
                    break;
                case DataControlCommands.CancelCommandName:
                    grdTeams.EditIndex = -1;
                    LoadGrid();
                    break;
                case DataControlCommands.UpdateCommandName:
                     grdTeams.UpdateRow(rowIndex, true);
                    grdTeams.EditIndex = -1;
                    LoadGrid();
                    break;
                case DataControlCommands.DeleteCommandName:                    
                    teamId = Convert.ToInt32(((GridView)sender).DataKeys[rowIndex][0]);
                    DeleteTeam(teamId);
                    LoadGrid();
                    break;
                case DataControlCommands.InsertCommandName:

                    break;
                case "LoadMembers":
                    teamId = Convert.ToInt32(((GridView)sender).DataKeys[rowIndex][0]);
                    Team myTeam = tmRepo.FindBy(teamId);
                    LoadMemberGrid(myTeam.Members);
                    hdfShowModal.Value = "1";
                    break;
                default:
                    break;
            }
            e.Handled = true;
        }

        void LoadMemberDropDownList(DropDownList ddl,int memberId = -1)
        {
            if (!Equals(ddl, null))
            {                
                ddl.DataSource = new TeamMemberRepository().GetAll();
                ddl.DataTextField = "FullName";
                ddl.DataValueField = "TeamMemberId";
                ddl.DataBind();

                ddl.SelectedValue = memberId.ToString();
            }
        }


        void DeleteTeam(int teamId)
        {
            tmRepo.Delete(teamId);
            tmRepo.Save();
        }

        protected void grdTeams_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState == DataControlRowState.Edit) || e.Row.RowState == (DataControlRowState.Alternate | DataControlRowState.Edit))
                {
                    var ddl = e.Row.FindControl("ddlLeader");
                    LoadMemberDropDownList((DropDownList)ddl, ((Team)e.Row.DataItem).TeamLeaderId);
                }
            }
        }

        protected void grdTeams_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Team editedTeam = Convert.ToInt32(e.Keys[0]) == 0 ? new Team() : tmRepo.FindBy(e.Keys[0]);
            editedTeam.Name = e.NewValues["Name"].ToString();
            editedTeam.GroupNumber = e.NewValues["GroupNumber"].ToString();
            int newLeaderId = Convert.ToInt32(((DropDownList)((GridView)sender).Rows[e.RowIndex].FindControl("ddlLeader")).SelectedValue);
            editedTeam.TeamLeaderId = newLeaderId;
            if (Convert.ToInt32(e.Keys[0]) == 0)
            {
                tmRepo.Add(editedTeam);
            }
            else
            {
                tmRepo.Update(editedTeam);
            }
            tmRepo.Save();
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            List<Team> Teams = tmRepo.GetAll().ToList();
            var blank = new Team();
            Teams.Add(blank);
            grdTeams.EditIndex = Teams.IndexOf(blank);
            LoadGrid(Teams);
        }
    }
}