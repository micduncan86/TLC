<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="TLC._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <button id="btnReload" type="button" class="btn btn-primary btn-sm">
            <span class="glyphicon glyphicon-refresh"></span>
        </button>
    </h2>
    <div class="row">
        <div class="col-md-4">
            <div class="panel panel-default" id="pnlTeamInfo">
                <div class="panel-heading">
                    <span class="teamName">TEAM NAME HERE</span>
                    <span class="badge" style="float: right;"><span>#</span><span class="glyphicon glyphicon-user"></span></span>
                </div>
                <div class="panel-body">
                    <p class="teamNumber"></p>
                    <p class="teamLeader"></p>
                </div>
            </div>
        </div>
        <div class="col-md-8" id="pnlTeamEvents">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Events
                </div>
                <div class="panel-body">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th>Title</th>
                                <th>Date</th>
                                <th>Description</th>
                                <th>Status</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="5"><span class="glyphicon glyphicon-refresh spinning"></span></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12" id="pnlTeamMembers">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Members
                    <button id="btnReloadMembers" type="button" class="btn btn-primary btn-xs" style="float:right;">
                        <span class="glyphicon glyphicon-refresh"></span>
                    </button>
                </div>
                <div class="panel-body">
                    <table class="table table-striped">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Email</th>
                                <th>Phone</th>
                                <th>Address</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="5"><span class="glyphicon glyphicon-refresh spinning"></span></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnReloadMembers").click(function (e) {
                e.preventDefault();
                teams.getTeams(function () {
                    app.LoadMembers(teams.list.Members);
                });                
            });
            teams.init(function () {
                app.LoadTeamData(teams.list, function () {
                    app.LoadMembers(teams.list.Members, function () {
                        app.LoadEvent(teams.list.Events, function () {

                        });
                    });
                });
            });
        });
    </script>
</asp:Content>
