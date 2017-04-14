<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptTeamMonthlyReport.aspx.cs" Inherits="TLC.Reports.RptTeamMonthlyReport"  MasterPageFile="~/Site.Master" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="903px">
            <LocalReport ReportPath="RptTeamCheckUps.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="CheckUpDataSet" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="TLC.TeamMemberCheckUpDataSetTableAdapters.RPT_TeamCheckUpsTableAdapter" OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
</asp:Content>
