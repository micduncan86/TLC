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
        protected CacheManager cache;
        protected void Page_Load(object sender, EventArgs e)
        {
            cache = new CacheManager();
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
            string reportmimeType = string.Empty;
            string reportencoding = string.Empty;
            string reportextension = string.Empty;
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string[] streamIds;


            var report = new ReportRepository().GetReportByName(ReportName);
            if (report != null)
            {
                Microsoft.Reporting.WebForms.ReportDataSource data = new Microsoft.Reporting.WebForms.ReportDataSource();
                ReportParameters rptParams = null; 
                if (cache.CacheList.ContainsKey("rptParams"))
                {
                    rptParams = cache.CacheList["rptParams"] as ReportParameters;
                }
                data.Value = report.GetData(rptParams);
                data.Name = "DataSet1";
                ReportViewer1.LocalReport.DataSources.Add(data);
                ReportViewer1.LocalReport.ReportPath = string.Format("Reports/{0}.rdlc",report.FileName);                

            }
            byte[] reportPdf = ReportViewer1.LocalReport.Render("PDF", null, out reportmimeType, out reportencoding, out reportextension, out streamIds, out warnings);
            Response.Buffer = true;
            Response.Clear();      
            Response.ContentType = reportmimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + "reportPDF." + reportextension);
            Response.OutputStream.Write(reportPdf, 0, reportPdf.Length);
            Response.Flush();
            Response.End();
            //ReportViewer1.LocalReport.Refresh();
        }
    }
}
