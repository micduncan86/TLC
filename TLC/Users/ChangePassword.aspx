<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="TLC.Users.ChangePassword" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Password Change</h3>
    <div class="row">
        <div class="col-md-12">
                
            <asp:Panel ID="divForm" runat="server" CssClass="panel panel-default">
                <div class="panel-heading">
                    Change
                </div>
                <div class="panel-body">
                    <asp:Label ID="lblError" runat="server" class="btn btn-danger" Visible="false"></asp:Label>
                    <div>  
                        <div class="form-group">
                            <label>Email:</label>
                            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group" id="divOldPassword" runat="server">
                            <label>Orginial Password:</label>
                            <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>New Password:</label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <asp:LinkButton ID="lnkChange" runat="server" OnClick="lnkChange_Click" CssClass="btn btn-sm btn-warning">
                    <span class="glyphicon glyphicon-wrench"></span>
                    Change
                </asp:LinkButton>
                    <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="~/home.aspx" CssClass="btn btn-sm btn-info" Text="Back"></asp:HyperLink>                    
                </div>
            </asp:Panel>
        </div>
        <asp:Panel ID="divOutcome" runat="server" Visible="false">
            <asp:Label ID="lblOutcome" runat="server" class="btn btn-success" Text="You password has been changed."></asp:Label>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/home.aspx" CssClass="btn btn-sm btn-info" Text="Back"></asp:HyperLink> 
        </asp:Panel>
    </div>
</asp:Content>
