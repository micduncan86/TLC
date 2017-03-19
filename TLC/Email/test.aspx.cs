using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.Email
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtToEmail.Value))
            {
                if (!string.IsNullOrWhiteSpace(txtSubject.Value) && !string.IsNullOrWhiteSpace(txtBody.Value))
                {
                    var newEmail = TLC.Data.Email.Send(new List<string>() { txtToEmail.Value }, txtSubject.Value, txtBody.Value);
                    ((SiteMaster)Page.Master).AddNotification(Page, "Email Status", newEmail);
                }
            }
        }
    }
}