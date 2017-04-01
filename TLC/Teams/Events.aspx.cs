using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.Teams
{
    public partial class Events : System.Web.UI.Page
    {
        protected EventRepository evntRepo = new EventRepository();
        protected TeamRepository teamManager = new TeamRepository();  
        protected int teamId;
        protected SiteMaster master = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            master = ((SiteMaster)Page.Master);
            hdfShowModal.Value = "0";
            //Temporarily using a querystring for filtering viewable teams.
            //end goal will be to read from logged in user and what access they have to.
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                teamId = Convert.ToInt32(Request.QueryString["Id"].ToString());
            }
            if (!Page.IsPostBack)
            {

                //LoadGrid(GetTeams("api/team"));
                LoadGrid();
            }
        }
        void LoadGrid(object datasource = null)
        {
            if (Equals(datasource, null))
            {
                datasource = teamId > 0 ? evntRepo.GetAll().Where(x => x.TeamId == teamId).ToList() : evntRepo.GetAll();
            }
                        lstEvents.DataSource = datasource;
            lstEvents.DataBind();
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            LoadEventModal(null);
        }

        protected void LoadEventModal(Event myevent)
        {
            hdfShowModal.Value = "1";
            modalTitle.Text = myevent == null ? "Add New Event" : "Update Event";
            lnkAddEvent.CommandName = myevent == null ? "New" : "Update";
            lnkAddEvent.CommandArgument = myevent == null ? string.Empty : myevent.EventId.ToString();
            lnkAddEvent.Text = myevent == null ? "Add New Event" : "Update Event";
            txtEventTitle.Text = myevent == null ? "" : myevent.Title;
            txtEventDate.Text = myevent == null ? "" : myevent.EventDate.ToShortDateString();
            txtEventNotes.Text = myevent == null ? "" : myevent.Notes;
            chkIsComplete.Checked = myevent == null ? false : myevent.Completed;
            chkIsCancelled.Checked = myevent == null ? false : myevent.Cancelled;

            if (myevent != null && !myevent.Completed && !myevent.Cancelled)
            {
                chkIsPending.Checked = true;
            }

            LoadTeams(ddlTeam, null);
            ddlTeam.SelectedIndex = myevent == null ? -1 : ddlTeam.Items.IndexOf(ddlTeam.Items.FindByValue(myevent.TeamId.ToString()));
        }

        protected void LoadTeams(DropDownList ddl, object datasource = null)
        {
            if (ddl != null)
            {                    
                if (datasource == null)
                {
                    if (User.IsInRole(UserRepository.ReturnUserRole(Data.User.enumRole.Administrater)))
                    {
                        datasource = teamManager.GetAll().ToList();
                    }
                    else
                    {
                        datasource = teamManager.GetAll().Where(x => x.TeamId == master.loggedUser.MyTeamId).ToList();
                    }                 
                }

                ddl.DataSource = datasource;
                ddl.DataTextField = "TeamName";
                ddl.DataValueField = "TeamId";

                ddl.DataBind();
            }
        }
        protected void lstEvents_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            int rowIndex = e.Item.DataItemIndex;
            int eventId = Convert.ToInt32(((ListView)sender).DataKeys[rowIndex].Value);
            switch (e.CommandName)
            {
                case "Edit":
                    LoadEventModal(evntRepo.FindBy(eventId));
                    e.Handled = true;
                    break;
                case "Delete":
                    evntRepo.Delete(eventId);
                    evntRepo.Save();
                    master.AddNotification(Page, "Event Deleted", "You event has been removed.");
                    e.Handled = true;
                    LoadGrid();
                    break;
                default:
                    break;
            }

        }
        
        protected void lnkAddEvent_Click(object sender, EventArgs e)
        {
            var btn = sender as LinkButton;


            Event myEvent = new Event()
            {
                Title = txtEventTitle.Text,
                EventDate = Convert.ToDateTime(txtEventDate.Text),
                Notes = txtEventNotes.Text,
                Completed = chkIsComplete.Checked,
                Cancelled = chkIsCancelled.Checked,
                TeamId = ddlTeam.SelectedValue.Equals(string.Empty) ? 0 : Convert.ToInt32(ddlTeam.SelectedValue)
            };
 
            switch (btn.CommandName)
            {
                case "New":
                    evntRepo.Add(myEvent);
                    break;
                case "Update":
                    if (!string.IsNullOrEmpty(btn.CommandArgument))
                    {
                        myEvent.EventId = Convert.ToInt32(btn.CommandArgument);
                        evntRepo.Update(myEvent);
                    }


                    break;
                default:
                    break;
            }
            evntRepo.Save();
            LoadGrid();
        }
    }
}