using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.Teams
{
    public partial class Index : System.Web.UI.Page
    {
        protected TeamRepository tmRepo = new TeamRepository();
        protected int _teamId;
        protected void Page_Load(object sender, EventArgs e)
        {
            hdfShowModal.Value = "0";
            //Temporarily using a querystring for filtering viewable teams.
            //end goal will be to read from logged in user and what access they have to.
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                _teamId = Convert.ToInt32(Request.QueryString["Id"].ToString());
            }
            if (!Page.IsPostBack)
            {
                if (Request.Params.Get("Add") != null)
                {
                    lnkAdd_Click(null, null);
                }
                //LoadGrid(GetTeams("api/team"));
                LoadGrid();
            }
        }

        private HttpResponseMessage ApiCall(string path, string method = "get", object content = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Request.Url.GetLeftPart(UriPartial.Authority));
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                switch (method)
                {
                    case "Get":
                    case "get":
                        return client.GetAsync(path).Result;
                        break;
                    case "Post":
                    case "post":
                        return client.PostAsJsonAsync(path, content).Result;
                    case "Put":
                    case "put":
                        return client.PutAsJsonAsync(path, content).Result;
                    default:
                        return null;
                        break;

                }
            }
        }

        /// <summary>
        /// Calls the api controller TeamController to retrieve all teams
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected List<Team> GetTeams(string path)
        {
            List<Team> Teams = null;
            var response = ApiCall(path);
            if (response.IsSuccessStatusCode)
            {
                Teams = response.Content.ReadAsAsync<List<Team>>().Result;
            }
            return Teams;
        }
        void LoadGrid(object datasource = null)
        {
            if (Equals(datasource, null))
            {
                datasource = _teamId > 0 ? tmRepo.GetAll().Where(x => x.TeamId == _teamId).ToList() : tmRepo.GetAll();
            }
            lstTeams.DataSource = datasource;
            lstTeams.DataBind();
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

        protected void lstTeams_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            int rowIndex = e.Item.DataItemIndex;
            int teamId = Convert.ToInt32(((ListView)sender).DataKeys[rowIndex].Value);            
            switch (e.CommandName)
            {
                case DataControlCommands.EditCommandName:
                    Team myTeam = tmRepo.FindBy(teamId);
                    pnlNewTeam.Visible = true;
                    pnlMembers.Visible = false;
                    hdfShowModal.Value = "1";
                    LoadMemberDropDownList(ddlNewTeamLeader, myTeam.TeamLeaderId);
                    txtNewTeamGroupNumber.Text = myTeam.TeamNumber;
                    txtNewTeamName.Text = myTeam.TeamName;
                    lnkAddNewTeam.Visible = true;
                    ltrModalTitle.Text = "Update Team :" + myTeam.TeamName;
                    lnkAddNewTeam.CommandName = "Update";
                    lnkAddNewTeam.CommandArgument = myTeam.TeamId.ToString();
                    lnkAddNewTeam.Text = "Update Team";
                    lnkManageMembers.Visible = false;
                    LoadGrid();
                    break;   
                case DataControlCommands.DeleteCommandName:
                    DeleteTeam(teamId);
                    LoadGrid();
                    break;
                case "LoadMembers":
                    teamId = Convert.ToInt32(((GridView)sender).DataKeys[rowIndex][0]);
                    lnkManageMembers.NavigateUrl = "Members.aspx?Id=" + teamId;
                    myTeam = tmRepo.FindBy(teamId);
                    LoadMemberGrid(myTeam.Members);
                    pnlMembers.Visible = true;
                    pnlNewTeam.Visible = false;
                    lnkAddNewTeam.Visible = false;
                    lnkManageMembers.Visible = true;
                    hdfShowModal.Value = "1";
                    break;
                default:
                    break;
            }
            e.Handled = true;
        }

        void LoadMemberDropDownList(DropDownList ddl, int memberId = -1, object datasource = null)
        {
            if (!Equals(ddl, null))
            {

                ddl.DataSource = Equals(datasource, null) ? new UserRepository().GetAll() : datasource;
                ddl.DataTextField = "UserName";
                ddl.DataValueField = "UserId";
                ddl.DataBind();

                ddl.Items.Insert(0,new ListItem("", "-1"));

                if (memberId != -1 && (ddl.Items.FindByValue(memberId.ToString()) != null))
                {
                    ddl.SelectedValue = memberId.ToString();
                }
            }
        }


        void DeleteTeam(int teamId)
        {
            Team myTeam = tmRepo.FindBy(teamId);
            SiteMaster master = ((SiteMaster)Page.Master);
            if (myTeam.Members.Count == 0)
            {
                tmRepo.Delete(teamId);
                tmRepo.Save();
                master.AddNotification(Page, "Team Deleted", myTeam.TeamName + " has been removed.");
            }
            else
            {
                master.AddNotification(Page, "Team Delete Failed", "Cannot deleted this team because it has member assigned to it.");
            }         
        }


        protected void grdTeams_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Team editedTeam = Convert.ToInt32(e.Keys[0]) == 0 ? new Team() : tmRepo.FindBy(e.Keys[0]);
            editedTeam.TeamNumber = e.NewValues["GroupNumber"].ToString();
            int newLeaderId = Convert.ToInt32(((DropDownList)((GridView)sender).Rows[e.RowIndex].FindControl("ddlLeader")).SelectedValue);
            string name = ((TextBox)((GridView)sender).Rows[e.RowIndex].FindControl("txtName")).Text;
            editedTeam.TeamName = name;
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
            pnlNewTeam.Visible = true;
            pnlMembers.Visible = false;
            hdfShowModal.Value = "1";
            LoadMemberDropDownList(ddlNewTeamLeader, -1);
            lnkAddNewTeam.Visible = true;
            ltrModalTitle.Text = "Add New Team";
            txtNewTeamName.Text = "";
            txtNewTeamGroupNumber.Text = "";
            lnkAddNewTeam.CommandName = "New";
            lnkAddNewTeam.CommandArgument = string.Empty;
            lnkAddNewTeam.Text = "Add New Team";
            lnkManageMembers.Visible = false;
        }

        protected void lnkAddNewTeam_OnClick(object sender, EventArgs e)
        {
            var btn = sender as LinkButton;
            var provider = new UserRepository();
            Team updateTeam = new Team()
            {
                TeamName = txtNewTeamName.Text,
                TeamNumber = txtNewTeamGroupNumber.Text,
                TeamLeaderId = ddlNewTeamLeader.SelectedValue.Equals(string.Empty) ? -1 : Convert.ToInt32(ddlNewTeamLeader.SelectedValue)
            };
            int id = 0;
            if (!string.IsNullOrEmpty(btn.CommandArgument))
                id = Convert.ToInt32(btn.CommandArgument);         
            var TeamRepo = new TeamRepository();

            switch (btn.CommandName)
            {
                case "New":
                    TeamRepo.Add(updateTeam);
                    TeamRepo.Save();                
                 
                    ((SiteMaster)Page.Master).AddNotification(Page, "Team Update Successful", updateTeam.TeamName + " was updated.");
                    break;
                case "Update":
                         
                    var team = TeamRepo.FindBy(id);
                    team.TeamName = updateTeam.TeamName;
                    team.TeamNumber = updateTeam.TeamNumber;
                    team.TeamLeaderId = updateTeam.TeamLeaderId;                                          

                    TeamRepo.Update(team);
                    TeamRepo.Save();
                    ((SiteMaster)Page.Master).AddNotification(Page, "Team Update Successful", updateTeam.TeamName + " was updated.");
                    break;
                default:
                    break;
            }
            if (Request.Params.Get("Add") != null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Page.UniqueID + "_Notification", "", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Page.UniqueID + "_ModalNotification", string.Format("app.modalFunction('{0}','{1}',{2},{3});", "Team Added", "A New Team has been added.", "null", "function(){ window.location='../home.aspx';}"), true);
                
            }
            LoadGrid();
            
        }

        protected void ChangeLeader(Team myteam,int newLeaderId)
        {
            var provider = new UserRepository();
            var tmRepo = new TeamRepository();
            if (myteam.TeamLeaderId != newLeaderId)
            {
                var oldLeader = provider.FindBy(myteam.TeamLeaderId);
                if (oldLeader != null)
                {
                    if (oldLeader.MyTeamId != myteam.TeamId)
                    {
                        var oldTeam = tmRepo.FindBy(oldLeader.MyTeamId);
                        if (oldTeam != null)
                        {
                            oldTeam.TeamLeaderId = -1;
                            tmRepo.Update(oldTeam);
                            tmRepo.Save();
                        }
                        
                    }
                    oldLeader.MyTeamId = -1;
                    provider.Update(oldLeader);
                    provider.Save();
                }
            }
            myteam.TeamLeaderId = newLeaderId;
            var newLeader = provider.FindBy(newLeaderId);
            if (newLeader != null)
            {
                if (newLeader.MyTeamId != myteam.TeamId)
                {
                    var oldTeam = tmRepo.FindBy(newLeader.MyTeamId);
                    if (oldTeam != null)
                    {
                        oldTeam.TeamLeaderId = -1;
                        tmRepo.Update(oldTeam);
                        tmRepo.Save();
                    }
                }
                newLeader.MyTeamId = myteam.TeamId;
                provider.Update(newLeader);
                provider.Save();
            }
        }

    }
}