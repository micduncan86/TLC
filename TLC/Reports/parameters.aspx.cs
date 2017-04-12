using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.Reports
{
    public partial class parameters : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillReports(ddlReports);
            }
        }

        private void FillReports(DropDownList ddl)
        {
            var reports = TLC.Data.Reports.GetReports();
            foreach(Report _rpt in reports)
            {
                ddl.Items.Add(new ListItem(_rpt.Name));
            }
            ddl.Items.Insert(0, new ListItem(""));
        }

    }
}