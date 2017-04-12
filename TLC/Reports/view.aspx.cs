using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.Reports
{
    public partial class view : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string rptName = Request.Params.Get("Report");
                LoadReport(rptName);
            }
        }
        private void LoadReport(string ReportName)
        {
            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            ReportViewer1.LocalReport.DataSources.Clear();


            var report = TLC.Data.Reports.GetReports().Where(x => x.Name == ReportName).FirstOrDefault();
            if (report != null)
            {
                Microsoft.Reporting.WebForms.ReportDataSource data = new Microsoft.Reporting.WebForms.ReportDataSource();
                data.Value = TLC.Data.Reports.GetData(report.ReportType);
                data.Name = "DataSet1";
                ReportViewer1.LocalReport.DataSources.Add(data);
                ReportViewer1.LocalReport.ReportPath = string.Format("Reports/{0}.rdlc",report.ReportFile);                

            }            
            ReportViewer1.LocalReport.Refresh();
        }
    }
}