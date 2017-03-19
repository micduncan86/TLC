<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="TLC.Teams.Index" MasterPageFile="~/Site.Master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Teams        
        <asp:LinkButton ID="lnkAdd" runat="server" CssClass="btn btn-sm btn-success" Style="float: right;" OnClick="lnkAdd_Click">
            <span class="glyphicon glyphicon-plus"></span>
            Add New Team
        </asp:LinkButton>
    </h3>
    <div>
        <div class="row">
            <div class="col-md-12">
                <asp:ListView ID="lstTeams" runat="server" DataKeyNames="TeamId" GroupPlaceholderID="phGroup" ItemPlaceholderID="phItem" OnItemCommand="lstTeams_ItemCommand">
                    <EmptyDataTemplate>
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <table class="table table-striped table-hover" style="margin-bottom: unset;">
                                    <thead>
                                        <tr>
                                            <th style="border-bottom: unset; border-right: 1px solid silver; min-width: 100px; width: 100px;"></th>
                                            <th class="col-md-2 col-mobile-only" style="border-bottom: unset; border-right: 1px solid silver;">Team</th>
                                            <th class="col-md-3 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Team</th>
                                            <th class="col-md-1 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Number</th>
                                            <th class="col-md-6 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Leaders</th>

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
                                            <th class="col-md-2 col-mobile-only" style="border-bottom: unset; border-right: 1px solid silver;">Team</th>
                                            <th class="col-md-3 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Team</th>
                                            <th class="col-md-1 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Number</th>
                                            <th class="col-md-6 col-mobile" style="border-bottom: unset; border-right: 1px solid silver;">Leaders</th>

                                        </tr>
                                    </thead>
                                </table>
                            </div>
                            <div class="panel-body" style="min-height:400px; max-height:400px; overflow:auto;">
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
                                <button class="btn btn-sm btn-info dropdown-toggle" type="button" data-toggle="dropdown">
                                    Actions
                                <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu" role="menu">
                                    <li role="presentation">
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit">
                                        <span class="glyphicon glyphicon-pencil btn btn-xs btn-info"></span>
                                         Edit
                                        </asp:LinkButton>
                                    </li>
                                    <li role="presentation">
                                        <a href="../home.aspx?TeamId=<%# Eval("TeamId") %>">
                                            <span class="glyphicon glyphicon-th-list btn btn-xs btn-info"></span>
                                            Member List
                                        </a>
                                    </li>
                                    <li role="presentation">
                                        <a href='<%# "Events.aspx?Id=" + ((TLC.Data.Team)DataBinder.GetDataItem(Container)).TeamId %>'>
                                            <span class="glyphicon glyphicon-calendar btn btn-xs btn-info"></span>
                                            Events</a>
                                    </li>
                                    <li role="presentation" class="divider"></li>
                                    <% if (HttpContext.Current.User.IsInRole("Administrater"))
                                        { %>               
                                        <li role="presentation" id="liDelete" runat="server">
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" OnClientClick="javascript: return confirm('Are you sure you want to delete this member?');">
                                        <span class="glyphicon glyphicon-trash btn btn-xs btn-danger"></span>
                                         Delete
                                        </asp:LinkButton>
                                    </li>
                                    <%} %>
                                </ul>
                            </div>
                        </td>
                        <td class="col-md-2 col-mobile-only">
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
                        </td>
                        <td class="col-md-3 col-mobile">
                            <%# Eval("TeamName") %>                    
                        </td>
                        <td class="col-md-1 col-mobile">
                            <%# Eval("TeamNumber") %>                            
                        </td>
                        <td class="col-md-6 col-mobile">
                            <%# Eval("TeamLeader.UserName") %>
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
                        <asp:Literal ID="ltrModalTitle" runat="server">Team Members</asp:Literal></h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="pnlMembers" runat="server">
                        <asp:ListView ID="lstMembers" runat="server" GroupPlaceholderID="grpContent" ItemPlaceholderID="itmContent">
                            <EmptyDataTemplate>
                                No Members
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <ul class="list-group">
                                    <asp:PlaceHolder ID="grpContent" runat="server"></asp:PlaceHolder>
                                </ul>
                            </LayoutTemplate>
                            <GroupTemplate>
                                <li class="list-group-item">
                                    <asp:PlaceHolder ID="itmContent" runat="server"></asp:PlaceHolder>
                                </li>
                            </GroupTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="ltrMember" runat="server" Text='<%# Eval("FullName") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                    <asp:Panel ID="pnlNewTeam" runat="server" Visible="False">
                        <p>
                            <label>Team Name:</label>
                            <asp:TextBox runat="server" ID="txtNewTeamName" CssClass="form-control"></asp:TextBox>
                        </p>
                        <p>
                            <label>Group Number:</label>
                            <asp:TextBox runat="server" ID="txtNewTeamGroupNumber" CssClass="form-control"></asp:TextBox>
                        </p>
                        <p>
                            <label>Team Leader:</label>
                            <asp:DropDownList ID="ddlNewTeamLeader" runat="server" CssClass="form-control"></asp:DropDownList>
                        </p>
                    </asp:Panel>
                </div>
                <div class="modal-footer">
                    <asp:HyperLink ID="lnkManageMembers" runat="server" CssClass="btn btn-sm btn-success">
                        <span class="glyphicon glyphicon-circle"></span>
                        Manage Members
                    </asp:HyperLink>
                    <asp:LinkButton runat="server" ID="lnkAddNewTeam" CommandName="New" CssClass="btn btn-sm btn-success" Visible="False" OnClick="lnkAddNewTeam_OnClick">
                        <span class="glyphicon glyphicon-plus"></span>
                        Add Team
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
            }
        });
    </script>
</asp:Content>
