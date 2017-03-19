using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic.FileIO;

namespace TLC.global
{
    public partial class import : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        protected List<string> DBFields()
        {
            return typeof(Data.Member).GetProperties(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Select(x => x.Name)
                .ToList();
        }
        protected List<Dictionary<string, object>> DataToJson(System.Data.DataTable dtdata)
        {
            List<Dictionary<string, object>> rtrn = new List<Dictionary<string, object>>();
            foreach (System.Data.DataRow row in dtdata.Rows)
            {
                Dictionary<string, object> drow = new Dictionary<string, object>();
                foreach (System.Data.DataColumn col in dtdata.Columns)
                {
                    drow.Add(col.ColumnName, row[col]);
                }
                rtrn.Add(drow);
            }
            return rtrn;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dtData = new System.Data.DataTable();
            if (fileUpload.HasFile)
            {
                using (TextFieldParser parser = new TextFieldParser(fileUpload.PostedFile.InputStream))
                {
                    parser.CommentTokens = new string[] { "#" };
                    parser.SetDelimiters(new string[] { "," });
                    parser.HasFieldsEnclosedInQuotes = false;

                    //read column line
                    if (dtData.Columns.Count == 0)
                    {
                        if (!parser.EndOfData)
                        {
                            foreach (string col in parser.ReadFields())
                            {
                                dtData.Columns.Add(col);
                            }
                        }
                    }
                    while (!parser.EndOfData)
                    {
                        Boolean addRow = false;
                        string[] fields = parser.ReadFields();
                        foreach (string field in fields)
                        {
                            if (!String.IsNullOrWhiteSpace(field))
                            {
                                addRow = true;
                                break;
                            }
                        }
                        if (addRow)
                        {
                            System.Data.DataRow newRow = dtData.NewRow();
                            for (int i = 0; i < fields.Length; i++)
                            {
                                newRow[i] = fields[i];
                            }
                            dtData.Rows.Add(newRow);
                            dtData.AcceptChanges();
                        }
                    }
                }


                System.Data.DataTable dtreturn = new System.Data.DataTable();


                Dictionary<string, object> rtrn = new Dictionary<string, object>();
                rtrn.Add("dbCols", DBFields());
                rtrn.Add("dataCols", dtData.Columns.OfType<System.Data.DataColumn>().Select(x => x.ColumnName).ToList());
                rtrn.Add("data", new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(DataToJson(dtData)));


                String json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(rtrn);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "importdata", string.Format(@"var importdata = {0}; ShowData(importdata);", json), true);

                //dgData.DataSource = dtData;
                //dgData.DataBind();
                //var y = DBFields();
            }
        }
    }
}