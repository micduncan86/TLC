<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TLC.Account.Login" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <br />
    <div class="row">
        <div class="col-md-8">
            <div class="panel panel-default">
                <div class="panel-heading">
                   <%: Title %>
                </div>
                <div class="panel-body">
                    <section id="loginForm">
                        <div class="form-horizontal">
                            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                                <p class="text-danger">
                                    <asp:Literal runat="server" ID="FailureText" />
                                </p>
                            </asp:PlaceHolder>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                                    <asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="Email"
                                        CssClass="text-danger" ErrorMessage="The email field is required." />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
                                </div>
                            </div>                   
                            <div class="form-group">
                                <div class="col-md-offset-2 col-md-10">
                                    <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-default" />
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
