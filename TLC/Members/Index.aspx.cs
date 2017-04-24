﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.Members
{
    public partial class INdex : System.Web.UI.Page
    {
        protected SiteMaster _master;
        protected MemberRepository memberRepo = new MemberRepository();
        protected int memberId;
        protected void Page_Load(object sender, EventArgs e)
        {
            _master = Page.Master as SiteMaster;
            hdfShowModal.Value = "0";
            if (!string.IsNullOrEmpty(Request.Params.Get("Id")))
            {
                memberId = Convert.ToInt32(Request.Params.Get("Id"));
            }
            if (!Page.IsPostBack)
            {

                LoadGrid();
                if (Request.Params.Get("Add") != null)
                {
                    lnkAdd_Click(null, null);
                }
            }
        }
        void LoadGrid(object datasource = null)
        {
            if (Equals(datasource, null))
            {

                if (User.IsInRole(TLC.Data.UserRepository.ReturnUserRole((Data.User.enumRole.Administrater))))
                {
                    datasource = memberRepo.GetAll();
                }
                else
                {
                    datasource = memberRepo.GetAll().Where(x => x.TeamId == _master.loggedUser.MyTeamId).ToList();
                }
            }

            if (memberId > 0)
            {
                datasource = ((List<Member>)datasource).Where(x => x.MemberId  == memberId).ToList();
            }

            int teamId;
            if (int.TryParse(Request.Params.Get("TeamId"), out teamId))
            {
                datasource = ((List<Member>)datasource).Where(x => x.TeamId <= 0).ToList();
            }


            lstMembers.DataSource = ((List<Member>)datasource).OrderBy(x => x.FullName).ToList();
            lstMembers.DataBind();
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {

            ModalMemberWindow(null);
        }

        private void ModalMemberWindow(Member curMember)
        {
            pnlNewTeam.Visible = true;
            hdfShowModal.Value = "1";
            ltrModalTitle.Text = curMember == null ? "Add New Member" : "Update Member";
            txtNewMemberName.Text = curMember == null ? "" : curMember.FullName;
            txtNewMemberPhone.Text = curMember == null ? "" : curMember.Phone;
            txtNewMemberEmail.Text = curMember == null ? "" : curMember.Email;
            txtNewMemberAddress.Text = curMember == null ? "" : curMember.Address;
            txtNewMemberCity.Text = curMember == null ? "" : curMember.City;
            txtNewMemberState.Text = curMember == null ? "" : curMember.State;
            txtNewMemberZipCode.Text = curMember == null ? "" : curMember.ZipCode;
            divteamselection.Visible = true;
            ddlTeam.SelectedIndex = -1;
            if (User.IsInRole(UserRepository.ReturnUserRole(Data.User.enumRole.Administrater)))
            {
                ddlTeam.DataSource = (from teams in new TeamRepository().GetAll()
                                      select teams).ToList();
            }
            else
            {
                int _teamId = 0;
                if (int.TryParse(Request.Params.Get("TeamId"), out _teamId))
                {
                    ddlTeam.DataSource = (from teams in new TeamRepository().GetAll()
                                          where teams.TeamId == _teamId
                                          select teams).ToList();
                }
                else
                {
                    divteamselection.Visible = false;
                }
            }

            ddlTeam.DataTextField = "TeamName";
            ddlTeam.DataValueField = "TeamId";
            ddlTeam.DataBind();
            ddlTeam.Items.Insert(0, new ListItem("", "0"));

            if (ddlTeam.Items.FindByValue(curMember == null ? "0" : curMember.TeamId.ToString()) != null)
            {
                ddlTeam.SelectedValue = curMember == null ? "0" : curMember.TeamId.ToString();
            }


            lnkAddUpdateMember.CommandName = curMember == null ? "New" : "Update";
            lnkAddUpdateMember.CommandArgument = curMember == null ? string.Empty : curMember.MemberId.ToString();
            lnkAddUpdateMember.Text = curMember == null ? "Add New Member" : "Update Member";
        }

        protected void lnkAddUpdateMember_Click(object sender, EventArgs e)
        {
            var btn = sender as LinkButton;

            List<string> name = txtNewMemberName.Text.Split(' ').ToList();
            Member member = new Member()
            {
                FirstName = name.First(),
                Phone = txtNewMemberPhone.Text,
                Email = txtNewMemberEmail.Text,
                Address = txtNewMemberAddress.Text,
                City = txtNewMemberCity.Text,
                State = txtNewMemberState.Text,
                ZipCode = txtNewMemberZipCode.Text,
                TeamId = ddlTeam.SelectedValue.Equals(string.Empty) ? 0 : Convert.ToInt32(ddlTeam.SelectedValue)
            };
            name.RemoveAt(0);
            member.LastName = string.Join(" ", name.ToArray());

            switch (btn.CommandName)
            {
                case "New":
                    memberRepo.Add(member);
                    memberRepo.Save();
                    break;
                case "Update":
                    if (!string.IsNullOrEmpty(btn.CommandArgument))
                    {
                        member.MemberId = Convert.ToInt32(btn.CommandArgument);
                        memberRepo.Update(member);
                    }


                    break;
                default:
                    break;
            }
            memberRepo.Save();

            if (Request.Params.Get("Add") != null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Page.UniqueID + "_ModalNotification", string.Format("app.modalFunction('{0}','{1}',{2},{3});", "Member Added", "A New Member has been added.", "null", "function(){ window.location='../home.aspx';}"), true);
            }
            else
            {
                ((SiteMaster)Page.Master).AddNotification(Page, "Member Update Successful", member.FullName + " was updated.");
            }
            LoadGrid();
        }
        protected void lstMembers_ItemDataBound1(object sender, ListViewItemEventArgs e)
        {

            //TO DO: WHEN LOGGED IN AS NORMAL USER, NOT ABLE TO EDIT MEMBERS OF OTHER TEAMS,
            //ONLY ALLOW UPDATES TO MY MEMBERS. COMPARE ROW TEAMID to LOGIN TEAMID


            var liAssign = e.Item.FindControl("liAssign");
            var liDelete = e.Item.FindControl("liDelete");
            var liEdit = e.Item.FindControl("liEdit");
            var lnkTeam = e.Item.FindControl("lnkTeam") as HyperLink;
            var lnkTeamSummary = e.Item.FindControl("lnkTeamSummary") as HyperLink;
            

            var member = (Member)e.Item.DataItem;
            Team myTeam = null;
            if (member != null)
            {
                myTeam = new TeamRepository().FindBy(member.TeamId);
            }    
            if (liAssign != null)
            {
                int teamId;
                if (int.TryParse(Request.Params.Get("TeamId"), out teamId))
                {
                    liAssign.Visible = true;
                }
            }
            if (liDelete != null)
            {
                if (!User.IsInRole(UserRepository.ReturnUserRole(Data.User.enumRole.Administrater)))
                {
                    liDelete.Visible = false;
                }
            }

            if (myTeam != null)
            {
                lnkTeam.Text = myTeam.TeamName;
                lnkTeam.NavigateUrl = "~/home.aspx?TeamId=" + myTeam.TeamId;

                lnkTeam.Visible = !String.IsNullOrWhiteSpace(lnkTeam.Text);

                lnkTeamSummary.Text = lnkTeam.Text;
                lnkTeamSummary.NavigateUrl = lnkTeam.NavigateUrl;
                lnkTeamSummary.Visible = !String.IsNullOrWhiteSpace(lnkTeamSummary.Text);

                if (liEdit != null && !User.IsInRole(UserRepository.ReturnUserRole(Data.User.enumRole.Administrater)))
                {
                    if (myTeam.TeamId != _master.loggedUser.MyTeamId)
                    {
                        liEdit.Visible = false;
                    }                    
                }   
            }
            else
            {
                lnkTeam.Visible = false;
                lnkTeamSummary.Visible = false;
            }


        }
        protected void Search(string searchTerm)
        {
            List<Member> list = null;
            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                list = (from member in new MemberRepository().GetAll()
                        where (1 == 1)
                        && (
                        (member.FullName.ToLower() ?? "").Contains(searchTerm.ToLower())
                        || (member.Email.ToLower() ?? "").Contains(searchTerm.ToLower())
                        || (member.Phone ?? "").Contains(searchTerm)
                        )
                        orderby member.FullName
                        select member).ToList();
            }
            LoadGrid(list);
        }
        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            Search(txtsearch.Value);
        }

        protected void lstMembers_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            var grid = sender as ListView;
            int teamMemberId = Convert.ToInt32(grid.DataKeys[e.Item.DataItemIndex].Value);
            switch (e.CommandName)
            {
                case "Edit":
                    ModalMemberWindow(memberRepo.FindBy(teamMemberId));
                    break;
                case "Assign":
                    Member selMember = memberRepo.FindBy(teamMemberId);
                    int teamId;
                    int.TryParse(Request.Params.Get("TeamId"), out teamId);
                    selMember.TeamId = teamId;
                    memberRepo.Update(selMember);
                    memberRepo.Save();
                    ((SiteMaster)Page.Master).AddNotification(Page, "Member Assignment Successful", selMember.FullName + " was added to your team.");
                    break;
                case "Copy":
                    ModalMemberWindow(memberRepo.FindBy(teamMemberId));
                    lnkAddUpdateMember.CommandName = "New";
                    lnkAddUpdateMember.CommandArgument = string.Empty;
                    lnkAddUpdateMember.Text = "Copy Member";
                    ltrModalTitle.Text = "Copy Member";
                    break;
                case "Delete":
                    memberRepo.Delete(teamMemberId);
                    memberRepo.Save();
                    ((SiteMaster)Page.Master).AddNotification(Page, "Member Removal Successful", "Member was removed.");
                    break;
                default:
                    break;
            }
            e.Handled = true;
            LoadGrid();
        }


    }
}