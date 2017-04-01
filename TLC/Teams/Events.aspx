<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Events.aspx.cs" Inherits="TLC.Teams.Events" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" href="../Content/bootstrap-datepicker.css" />

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Events 
        <a class="btn btn-sm btn-info" style="float: right; margin: 0 5px;" href="../home.aspx?TeamId=<% Response.Write(Request.Params.Get("Id")); %>">
            <span class="glyphicon glyphicon-menu-left"></span>
            Back to Team
        </a>
        <asp:LinkButton ID="lnkAdd" runat="server" CssClass="btn btn-sm btn-success" Style="float: right;" OnClick="lnkAdd_Click">
            <span class="glyphicon glyphicon-plus"></span>Add New Event
        </asp:LinkButton>
    </h3>
    <div>
        <div class="row">
            <div class="col-md-12">
                <asp:ListView ID="lstEvents" runat="server" DataKeyNames="EventId" GroupPlaceholderID="phGroup" ItemPlaceholderID="phItem" OnItemCommand="lstEvents_ItemCommand">
                    <EmptyDataTemplate>
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <table class="table table-striped table-hover" style="margin-bottom: unset;">
                                    <thead>
                                        <tr>
                                            <th style="border-bottom: unset; border-right: 1px solid silver; min-width: 100px; width: 100px;"></th>
                                            <th class="col-md-1 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Date</th>
                                            <th class="col-md-2 col-mobile-only" style="border-bottom: unset; border-right: 1px solid silver;">Title</th>                                            
                                            <th class="col-md-4 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Description</th>
                                            <th class="col-md-4 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Notes</th>
                                            <th class="col-md-1 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Status</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                            <div class="panel-body">
                                <strong>No Users</strong>
                            </div>
                        </div>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <table class="table table-striped table-hover" style="margin-bottom: unset;">
                                    <thead>
                                        <tr>
                                            <th style="border-bottom: unset; border-right: 1px solid silver; min-width: 100px; width: 100px;"></th>
                                            <th class="col-md-1 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Date</th>
                                            <th class="col-md-2 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Title</th>                         
                                            <th class="col-md-4 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Description</th>
                                            <th class="col-md-4 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Notes</th>
                                            <th class="col-md-1 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Status</th>

                                        </tr>
                                    </thead>
                                </table>
                            </div>
                            <div class="panel-body" style="min-height: 400px; max-height: 400px; overflow: auto;">
                                <table class="table table-striped table-hover">
                                    <tbody>
                                        <asp:PlaceHolder ID="phGroup" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </LayoutTemplate>
                    <GroupTemplate>
                        <tr>
                            <asp:PlaceHolder ID="phItem" runat="server"></asp:PlaceHolder>
                        </tr>
                    </GroupTemplate>
                    <ItemTemplate>
                        <td style="width: 90px;">
                            <div class="dropdown">
                                <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                                    Actions <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu" role="menu">
                                    <li role="presentation">
                                        <asp:LinkButton ID="LinkButton1" runat="server"  CommandName="Edit">
                                        <span class="glyphicon glyphicon-pencil btn btn-xs btn-info"></span>
                                        Edit
                                        </asp:LinkButton>
                                    </li>
                                    <li role="presentation" class="divider"></li>
                                    <li role="presentation">
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Delete" OnClientClick="javascript: return confirm('Are you sure you want to delete this team?');">
                                        <span class="glyphicon glyphicon-trash btn btn-xs btn-danger"></span>
                                        Delete
                                        </asp:LinkButton>
                                    </li>
                                </ul>
                            </div>
                        </td>
                        <%--       <td class="col-md-2 col-mobile-only">
                            <h4><%# Eval("TeamName") %></h4>
                            <div class="dropdown">
                                <button class="btn btn-info dropdown-toggle" type="button" data-toggle="dropdown">
                                    Info <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu" role="menu" style="padding: 5px;">
                                    <li role="presentation">
                                        <label>Team Leader:</label>
                                        <%# Eval("TeamLeader.UserName") %>
                                    </li>
                                    <li role="presentation">
                                        <label>Group Number:</label>
                                        <%# Eval("TeamNumber") %>
                                    </li>
                                    <li role="presentation">
                                        <label>Members:</label>
                                        <a href="../home.aspx?TeamId=<%# Eval("TeamId") %>" class="btn btn-xs btn-warning">
                                            <span class="glyphicon glyphicon-user"></span><%# Eval("Members.Count") %>
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </td>--%>
                        <td class="col-md-1 col-mobile">
                            <%# Eval("EventDate", "{0:d}") %>                    
                        </td>
                        <td class="col-md-2 col-mobile">
                            <%# Eval("Title") %>                    
                        </td>                        
                        <td class="col-md-4 col-mobile">
                            <%# Eval("Description") %>                            
                        </td>
                        <td class="col-md-4 col-mobile">
                            <%# Eval("Notes") %>
                        </td>
                        <td class="col-md-1 col-mobile">
                            <%# (bool)Eval("Completed") ? "Completed" : (bool)Eval("Cancelled") ? "Cancelled" : "Pending" %>         
                        </td>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </div>

    <div id="mdlMembers" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;
                    </button>
                    <h4 class="modal-title">
                        <asp:Literal ID="modalTitle" runat="server">Event</asp:Literal>
                    </h4>
                </div>
                <div class="modal-body">
                    <div>
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtEventTitle" CssClass="form-control" placeholder="Title" Style="max-width: none; width: 425px;"></asp:TextBox>
                        </div>
                        <div class="pull-right">
                            <asp:DropDownList ID="ddlTeam" runat="server" CssClass="form-control" Width="250px"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <div class="input-group date" data-provider="datepicker">
                                <asp:TextBox ID="txtEventDate" runat="server" CssClass="form-control datepicker" data-date-format="mm/dd/yyyy" placeholder="Event Date"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="3" placeholder="Description:"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtEventNotes" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="2" placeholder="Notes:"></asp:TextBox>
                        </div>
                        <div class="form-group form-inline">
                            <asp:RadioButton ID="chkIsComplete" runat="server" CssClass="" GroupName="status" Text=" Is Event Complete?" />
                             <asp:RadioButton ID="chkIsCancelled" runat="server" CssClass="" GroupName="status" Text=" Is Event Cancelled?" />
                            <asp:RadioButton ID="chkIsPending" runat="server" CssClass="" GroupName="status" Text=" Is Event Pending?" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lnkAddEvent" runat="server" CssClass="btn btn-sm btn-success" OnClick="lnkAddEvent_Click">
                        <span class="glyphicon glyphicon-circle"></span>
                        Add Event
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfShowModal" ClientIDMode="Static" runat="server" />
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#hdfShowModal").val() == "1") {
                $("#mdlMembers").modal("show");
                $("#mdlMembers").on("shown.bs.modal", function () {
                    $(".datepicker").datepicker({
                        format: "mm/dd/yyyy",
                        startDate: "-1d"
                    });
                });
            }
        });
    </script>
    <script src="../Scripts/bootstrap-datepicker.js"></script>
</asp:Content>
