<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TLC._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="Scripts/teams.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
    <h1>ASP.NET</h1>
    <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
    <p><a href="http://asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
</div>
<div class="row">
    <div class="col-md-4">
        <h2>Teams</h2>
        <p>Access Team section.</p>
        <p><a class="btn btn-default" href="../Teams/Index.aspx">Go</a></p>
    </div>
    <div class="col-md-4">
        <h2>Members</h2>
        <p>Access Member List section.</p>
        <p><a class="btn btn-default" href="../Members/Index.aspx">Go</a></p>
    </div>
    <div class="col-md-4">
        <h2>Web Hosting</h2>
        <p>You can easily find a web hosting company that offers the right mix of features and price for your applications.</p>
        <p><a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301872">Learn more &raquo;</a></p>
    </div>

    <form method="post" action="api/login">
        <input type="submit" value="Login"/>
    </form>
</div>
</asp:Content>
