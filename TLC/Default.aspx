<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TLC._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="Scripts/teams.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Teams</h2>
    <ul class="list-group lstTeams">
    </ul>
    <p>
        <button id="btnAdd" class="btn btn-primary btn-sm" data-toggle="modal" data-target="#myModal">
            <span class="glyphicon glyphicon-plus"></span><span>Add New Team</span>
        </button>
        <button id="btnReload" class="btn btn-primary btn-sm">
            <span class="glyphicon glyphicon-refresh"></span>
        </button>
    </p>
    <div class="container">
        <div class="row col-lg-3 label-success">1</div>
        <div class="row col-lg-1 label-default">-</div>
        <div class="row col-lg-3 label-info">2</div>
        <div class="row col-lg-1 label-default">-</div>
        <div class="row col-lg-3 label-warning">3</div>

    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".btn-expand-members").click(function (e) {
                e.preventDefault();
                $(this).closest("ul").find(".ulMembers").toggle('slow');
            });
            $("#btnReload").click(function (e) {
                e.preventDefault();
                var ul = $(".lstTeams").empty();
                teams.getTeams(function (data) {
                    $.each(data, function (k, v) {
                        teams.newTeamListItem(v, function (li) {
                            ul.append(li);
                        });
                    });
                });
            }).click();
            $("#btnAdd").click(function (e) {
                e.preventDefault();
                teams.modalFunction("Add New Team", function () {
                    $.get("teams/add", function (data, status) {
                        var d = $(data).find(".form-group").html();
                        $("#myModal").find(".modal-body").empty().append($("<form/>", { html: d }));
                    });
                },
                    $("<button/>", { class: "btn btn-success btn-sm", html: "Add Team" })
                    .click(function (e) {
                        e.preventDefault();
                        var postdata = $("#myModal").find(".modal-body").find("form").serialize();
                        teams.addTeam(postdata,function (nteam) {
                            teams.newTeamListItem(nteam, function (li) {
                                $(".lstTeams").append(li);
                                $("#myModal").modal("hide");
                            });
                        });
                    }));
            });
        });
    </script>
</asp:Content>
