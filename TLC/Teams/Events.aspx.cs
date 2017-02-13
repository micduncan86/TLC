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
        protected int teamId;
        protected void Page_Load(object sender, EventArgs e)
        {
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
            grdEvents.DataSource = datasource;
            grdEvents.DataBind();
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
            chkIsComplete.Checked = myevent == null ? false : myevent.isComplete;
            LoadTeams(ddlTeam, null);
            ddlTeam.SelectedIndex = myevent == null ? -1 : ddlTeam.Items.IndexOf(ddlTeam.Items.FindByValue(myevent.TeamId.ToString()));
        }

        protected void LoadTeams(DropDownList ddl, object datasource = null)
        {
            if (ddl != null)
            {
                if (datasource == null)
                {
                    datasource = new TeamRepository().GetAll().ToList();
                }

                ddl.DataSource = datasource;
                ddl.DataTextField = "Name";
                ddl.DataValueField = "TeamId";

                ddl.DataBind();
            }
        }

        protected void grdEvents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var grid = sender as GridView;
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int eventId = Convert.ToInt32(((GridView)sender).DataKeys[rowIndex][0]);
            switch (e.CommandName)
            {
                case "Edit":
                    LoadEventModal(evntRepo.FindBy(eventId));
                    break;
                case "Delete":
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
                isComplete = chkIsComplete.Checked,
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