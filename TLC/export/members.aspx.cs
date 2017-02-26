using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLC.Data;

namespace TLC.export
{
    public partial class members : System.Web.UI.Page
    {
        MemberRepository memberManager = new MemberRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            switch (Request.Params.Get("Type"))
            {
                case "xls":
                    ExportExcel();
                    break;

            }
        }

        protected void ExportExcel()
        {
            Response.Clear();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=lifechurchmemberlists.xls");
            Response.ContentType = "application/vnd.ms-excel";

            DataTable dtexport = new DataTable();

            GridView excel = new GridView();
            excel.DataSource = memberManager.GetAll();
            excel.DataBind();
            excel.RenderControl(new HtmlTextWriter(Response.Output));

            Response.Flush();
            Response.End();

        }
    }
}