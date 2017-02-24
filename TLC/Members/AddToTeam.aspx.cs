using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.Members
{
    public partial class AddToTeam : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString.ToString().ToLower().Contains("modal"))
                {
                   var nav = this.Master.FindControl("divNav") as System.Web.UI.HtmlControls.HtmlContainerControl;
                    nav.Visible = false;
                }
                Search("");
            }
        }
        protected void Search(string searchTerm)
        {
            var list = new List<Member>();
            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                list = (from member in new MemberRepository().GetAll()
                            where (member.TeamId == -1)
                            && (
                            (member.FullName.ToLower() ?? "").Contains(searchTerm.ToLower())
                            || (member.Email.ToLower() ?? "").Contains(searchTerm.ToLower())
                            || (member.Phone ?? "").Contains(searchTerm)
                            )
                            orderby member.FullName
                            select member).ToList();
            }
            else
            {
                list = new MemberRepository().GetAll().Where(x => x.TeamId == -1).OrderBy(y => y.FullName).ToList();
            }
           
            lstMembers.DataSource = list;
            lstMembers.DataBind();
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            Search(txtsearch.Value.ToString());
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var memRepo = new MemberRepository();
            foreach(var item in lstMembers.Items)
            {
                var chk = item.FindControl("chkAdd") as CheckBox;
                if (chk.Checked)
                {
                    var addMember = memRepo.FindBy(lstMembers.DataKeys[item.DataItemIndex].Value);
                    addMember.TeamId = Convert.ToInt32(HttpUtility.ParseQueryString(Request.Url.Query).Get("Team"));
                    memRepo.Update(addMember);                    
                }
            }
            memRepo.Save();
            Search(txtsearch.Value.ToString());
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SUCCESSMSG", "app.SuccessAlert('Success','Members have been added to the team.');", true);
        }
    }
}