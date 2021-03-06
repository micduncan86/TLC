﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptTeamRoster.aspx.cs" Inherits="TLC.Reports.RptTeamRoster" MasterPageFile="~/Site.Master"%>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="378px" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="1071px">
            <LocalReport ReportPath="RptTeamRoster.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="TLC.TeamRosterDataSetTableAdapters.RPT_TeamRosterTableAdapter" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:Parameter DefaultValue="" Name="TeamId" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
  
</asp:Content>
