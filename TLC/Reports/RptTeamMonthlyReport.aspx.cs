using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TLC.Reports
{
    public partial class RptTeamMonthlyReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /* Trying to get two reports to display.
             PlaceHolder1.Controls.Clear();


            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.ReportPath = "RptTeamCheckUps.rdlc";
            Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource("TeamMemberCheckUpDataSet", ObjectDataSource2);
            viewer.LocalReport.DataSources.Add(rds);


            PlaceHolder1.Controls.Add(viewer);


            viewer.LocalReport.Refresh(); */
        }

    }
}