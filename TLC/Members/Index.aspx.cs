using System;
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
        protected MemberRepository memberRepo = new MemberRepository();
        protected int memberId;
        protected void Page_Load(object sender, EventArgs e)
        {
            hdfShowModal.Value = "0";
            if (!string.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                memberId = Convert.ToInt32(Request.QueryString["Id"].ToString());
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
                datasource = memberId > 0 ? memberRepo.GetAll().Where(x => x.MemberId == memberId).ToList() : memberRepo.GetAll();
            }
            grdMembers.DataSource = datasource;
            grdMembers.DataBind();
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
            ddlTeam.SelectedIndex = -1;
            ddlTeam.DataSource = (from teams in new TeamRepository().GetAll()
                                  select teams).ToList();
            ddlTeam.DataTextField = "Name";
            ddlTeam.DataValueField = "TeamId";
            ddlTeam.DataBind();
            ddlTeam.Items.Insert(0, new ListItem("", "0"));

            ddlTeam.SelectedValue = curMember == null ? "0" : curMember.TeamId.ToString();


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
            LoadGrid();
        }

        protected void grdMembers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var grid = sender as GridView;
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int teamMemberId = Convert.ToInt32(((GridView)sender).DataKeys[rowIndex][0]);
            switch (e.CommandName)
            {
                case "Edit":
                    ModalMemberWindow(memberRepo.FindBy(teamMemberId));
                    break;
                case "Copy":

                    Member curMember = memberRepo.FindBy(teamMemberId);
                    curMember.Copy();
                    break;
                case "Delete":
                    memberRepo.Delete(teamMemberId);
                    memberRepo.Save();
                    break;
                default:
                    break;
            }
            e.Handled = true;
            LoadGrid();
        }

        

    }
}