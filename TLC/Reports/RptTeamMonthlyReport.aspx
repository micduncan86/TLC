<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptTeamMonthlyReport.aspx.cs" Inherits="TLC.Reports.RptTeamMonthlyReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 211px">
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    </div>
        
        <br />
       


        <br />
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="903px">
            <LocalReport ReportPath="RptTeamCheckUps.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="CheckUpDataSet" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="TLC.TeamMemberCheckUpDataSetTableAdapters.RPT_TeamCheckUpsTableAdapter"></asp:ObjectDataSource>
    </form>
</body>
</html>
