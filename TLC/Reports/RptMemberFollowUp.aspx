<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptMemberFollowUp.aspx.cs" Inherits="TLC.Reports.RptMemberFollowUp" MasterPageFile="~/Site.Master"%>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="1007px" Height="497px">
            <LocalReport ReportPath="RptMemberFollowUp.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="TLC.MemberFollowUpDataSetTableAdapters.RPT_TeamMemberFollowUpTableAdapter"></asp:ObjectDataSource>
</asp:Content>
